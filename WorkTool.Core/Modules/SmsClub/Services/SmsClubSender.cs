using WorkTool.Core.Modules.Http.Extensions;

using Constants = WorkTool.Core.Modules.SmsClub.Helpers.Constants;

namespace WorkTool.Core.Modules.SmsClub.Services;

public class SmsClubSender<TParameters> where TParameters : notnull
{
    private readonly HttpClient httpClient;
    private readonly IReadOnlyDictionary<string, string> endpoints;
    private readonly SmsClubSenderOptions options;
    private readonly IDelay delay;

    public SmsClubSender(
        HttpClient httpClient,
        IReadOnlyDictionary<string, string> endpoints,
        SmsClubSenderOptions options,
        IDelay delay
    )
    {
        this.delay = delay.ThrowIfNull();
        this.options = options.ThrowIfNull();
        this.endpoints = new Dictionary<string, string>(endpoints);
        this.httpClient = httpClient.ThrowIfNull();
    }

    public async Task<SendSmsClubResponse> SendSmsAsync(SendSmsClubRequest clubRequest)
    {
        using var content = clubRequest.ToJsonHttpContent();
        var url = endpoints[Constants.SmsSendEndpointId];
        using var httpResponseMessage = await httpClient.PostAsync(url, content);
        httpResponseMessage.ThrowIfNotSuccess();
        var sendSmsClubResponse =
            await httpResponseMessage.Content.ReadFromJsonAsync<SendSmsClubResponse>();
        sendSmsClubResponse = sendSmsClubResponse.ThrowIfNull();

        return sendSmsClubResponse;
    }

    public async IAsyncEnumerable<SendSmsClubResponse> SendsSmsesAsync(
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
