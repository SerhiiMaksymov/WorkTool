namespace WorkTool.Core.Modules.SmsClub.Services;

public class SmsClubSender<TParameters> : ISmsClubSender<TParameters> where TParameters : notnull
{
    private readonly HttpClient httpClient;
    private readonly SmsSenderEndpointsOptions endpointsOptions;
    private readonly SmsSenderOptions options;
    private readonly IDelay delay;

    public SmsClubSender(
        HttpClient httpClient,
        SmsSenderEndpointsOptions endpointsOptions,
        SmsSenderOptions options,
        IDelay delay
    )
    {
        this.delay = delay;
        this.options = options;
        this.endpointsOptions = endpointsOptions;
        this.httpClient = httpClient;
    }

    public async Task<BalanceResponse> GetBalanceAsync()
    {
        var smsResponse = await httpClient.PostReadJsonThrowIfNotSuccessAsync<BalanceResponse>(
            endpointsOptions.SmsBalance
        );

        smsResponse = smsResponse.ThrowIfNull();

        return smsResponse;
    }

    public async Task<SmsResponse<ArraySuccessRequest>> GetOriginatorsAsync()
    {
        var smsResponse = await httpClient.PostReadJsonThrowIfNotSuccessAsync<ArraySmsResponse>(
            endpointsOptions.SmsOriginator
        );

        smsResponse = smsResponse.ThrowIfNull();

        return smsResponse;
    }

    public async Task<DictionarySmsResponse> GetSmsStatusAsync(IEnumerable<string> smsIds)
    {
        smsIds = smsIds.ThrowIfEmpty().ToArray();
        var request = new GetSmsStatusRequest() { SmsIds = smsIds.ThrowIfEmpty().ToArray() };

        var jsonDocument = await httpClient.PostReadJsonDocumentThrowIfNotSuccessAsync(
            endpointsOptions.SmsStatus,
            request
        );

        var infoElement = jsonDocument.RootElement
            .GetProperty(DictionarySmsResponse.SuccessRequestJsonPropertyName)
            .GetProperty(DictionarySuccessRequest.InfoJsonPropertyName);

        if (infoElement.ValueKind == JsonValueKind.Array)
        {
            throw new NotFoundSmsesException(smsIds);
        }

        var smsResponse = jsonDocument.Deserialize<DictionarySmsResponse>();
        smsResponse = smsResponse.ThrowIfNull();

        return smsResponse;
    }

    public async Task<DictionarySmsResponse> SendSmsAsync(SendSmsRequest request)
    {
        var smsResponse =
            await httpClient.PostReadJsonThrowIfNotSuccessAsync<DictionarySmsResponse>(
                endpointsOptions.SmsSend,
                request
            );
        smsResponse = smsResponse.ThrowIfNull();

        return smsResponse;
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
