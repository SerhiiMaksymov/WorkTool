namespace WorkTool.Core.Modules.SmsClub.Services;

public class SmsClubSender<TParameters> where TParameters : notnull
{
    private readonly HttpClient httpClient;
    private readonly SmsClubSenderEndpoints endpoints;
    private readonly SmsClubSenderOptions options;
    private readonly IDelay delay;

    public SmsClubSender(
        HttpClient httpClient,
        SmsClubSenderEndpoints endpoints,
        SmsClubSenderOptions options,
        IDelay delay
    )
    {
        this.delay = delay.ThrowIfNull();
        this.options = options.ThrowIfNull();
        this.endpoints = endpoints;
        this.httpClient = httpClient.ThrowIfNull();
    }

    public async Task<SmsClubResponse> GetSmsStatusAsync(IEnumerable<string> smsIds)
    {
        var request = new GetSmsStatusRequest() { SmsIds = smsIds.ToArray() };
        using var httpResponseMessage = await httpClient.PostAsync(
            endpoints.SmsStatusEndpoint,
            request
        );
        var smsClubResponse =
            await httpResponseMessage.Content.ReadFromJsonAsync<SmsClubResponse>();
        smsClubResponse = smsClubResponse.ThrowIfNull();

        return smsClubResponse;
    }

    public async Task<SmsClubResponse> SendSmsAsync(SendSmsClubRequest clubRequest)
    {
        using var content = clubRequest.ToJsonHttpContent();
        var url = endpoints.SmsSendEndpoint;
        using var httpResponseMessage = await httpClient.PostAsync(url, content);
        httpResponseMessage.ThrowIfNotSuccess();
        var smsClubResponse =
            await httpResponseMessage.Content.ReadFromJsonAsync<SmsClubResponse>();
        smsClubResponse = smsClubResponse.ThrowIfNull();

        return smsClubResponse;
    }

    public async IAsyncEnumerable<SmsClubResponse> SendsSmsesAsync(
        MessageItemsCollection<TParameters> messageItems
    )
    {
        var currentCount = (ushort)0;

        foreach (var item in messageItems)
        {
            currentCount++;
            var request = new SendSmsClubRequest()
            {
                Message = item.GetMessageValue(),
                Recipient = item.Message.RecipientName,
                PhoneNumbers = new[] { item.Message.RecipientId }
            };
            yield return await SendSmsAsync(request);

            if (currentCount == options.Count)
            {
                currentCount = 0;
                await delay.DelayAsync(options.Wait);
            }
        }
    }
}
