namespace WorkTool.Core.Modules.SmsClub.Services;

public class SmsClubSender<TParameters> : ISmsClubSender<TParameters> where TParameters : notnull
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

    public async Task<SmsResponse<Balance>> GetBalanceAsync()
    {
        using var httpResponseMessage = await httpClient.GetAsync(endpoints.SmsBalanceEndpoint);
        httpResponseMessage.ThrowIfNotSuccess();
        var smsClubResponse = await httpResponseMessage.ReadFromJsonAsync<SmsResponse<Balance>>();
        smsClubResponse = smsClubResponse.ThrowIfNull();

        return smsClubResponse;
    }

    public async Task<SmsResponse<ArraySuccessRequest>> GetOriginatorsAsync()
    {
        using var httpResponseMessage = await httpClient.GetAsync(endpoints.SmsOriginatorEndpoint);
        httpResponseMessage.ThrowIfNotSuccess();
        var smsClubResponse = await httpResponseMessage.ReadFromJsonAsync<ArraySmsResponse>();
        smsClubResponse = smsClubResponse.ThrowIfNull();

        return smsClubResponse;
    }

    public async Task<SmsResponse<DictionarySuccessRequest>> GetSmsStatusAsync(
        IEnumerable<string> smsIds
    )
    {
        var request = new GetSmsStatusRequest() { SmsIds = smsIds.ToArray() };
        var url = endpoints.SmsStatusEndpoint;
        using var httpResponseMessage = await httpClient.PostAsync(url, request);
        httpResponseMessage.ThrowIfNotSuccess();
        var smsClubResponse = await httpResponseMessage.ReadFromJsonAsync<DictionarySmsResponse>();
        smsClubResponse = smsClubResponse.ThrowIfNull();

        return smsClubResponse;
    }

    public async Task<SmsResponse<DictionarySuccessRequest>> SendSmsAsync(SendSmsRequest request)
    {
        var url = endpoints.SmsSendEndpoint;
        using var httpResponseMessage = await httpClient.PostAsync(url, request);
        httpResponseMessage.ThrowIfNotSuccess();
        var smsClubResponse = await httpResponseMessage.ReadFromJsonAsync<DictionarySmsResponse>();
        smsClubResponse = smsClubResponse.ThrowIfNull();

        return smsClubResponse;
    }

    public async IAsyncEnumerable<SmsResponse<DictionarySuccessRequest>> SendsSmsesAsync(
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
