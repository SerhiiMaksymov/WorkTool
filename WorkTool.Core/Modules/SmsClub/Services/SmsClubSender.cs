namespace WorkTool.Core.Modules.SmsClub.Services;

public class SmsClubSender<TParameters> where TParameters : notnull
{
    private readonly HttpClient httpClient;
    private readonly SmsSenderEndpoints endpoints;
    private readonly SmsSenderOptions options;
    private readonly IDelay delay;

    public SmsClubSender(
        HttpClient httpClient,
        SmsSenderEndpoints endpoints,
        SmsSenderOptions options,
        IDelay delay
    )
    {
        this.delay = delay;
        this.options = options;
        this.endpoints = endpoints;
        this.httpClient = httpClient;
    }

    public async Task<ArraySmsResponse> GetOriginatorsAsync()
    {
        using var httpResponseMessage = await httpClient.GetAsync(endpoints.SmsOriginatorEndpoint);
        httpResponseMessage.ThrowIfNotSuccess();
        var smsClubResponse = await httpResponseMessage.ReadFromJsonAsync<ArraySmsResponse>();
        smsClubResponse = smsClubResponse.ThrowIfNull();

        return smsClubResponse;
    }

    public async Task<DictionarySmsResponse> GetSmsStatusAsync(IEnumerable<string> smsIds)
    {
        var request = new GetSmsStatusRequest() { SmsIds = smsIds.ToArray() };
        var url = endpoints.SmsStatusEndpoint;
        using var httpResponseMessage = await httpClient.PostAsync(url, request);
        httpResponseMessage.ThrowIfNotSuccess();
        var smsClubResponse = await httpResponseMessage.ReadFromJsonAsync<DictionarySmsResponse>();
        smsClubResponse = smsClubResponse.ThrowIfNull();

        return smsClubResponse;
    }

    public async Task<DictionarySmsResponse> SendSmsAsync(SendSmsRequest request)
    {
        var url = endpoints.SmsSendEndpoint;
        using var httpResponseMessage = await httpClient.PostAsync(url, request);
        httpResponseMessage.ThrowIfNotSuccess();
        var smsClubResponse = await httpResponseMessage.ReadFromJsonAsync<DictionarySmsResponse>();
        smsClubResponse = smsClubResponse.ThrowIfNull();

        return smsClubResponse;
    }

    public async IAsyncEnumerable<DictionarySmsResponse> SendsSmsesAsync(
        MessageItemsCollection<TParameters> messageItems
    )
    {
        var currentCount = (ushort)0;

        foreach (var item in messageItems)
        {
            currentCount++;
            var request = new SendSmsRequest()
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
