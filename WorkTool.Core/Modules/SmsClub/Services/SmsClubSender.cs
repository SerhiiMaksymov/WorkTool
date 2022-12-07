using WorkTool.Core.Modules.Http.Extensions;

using Constants = WorkTool.Core.Modules.SmsClub.Helpers.Constants;

namespace WorkTool.Core.Modules.SmsClub.Services;

public class SmsClubSender<TParameters> where TParameters : notnull
{
    private readonly HttpClient _httpClient;
    private readonly IReadOnlyDictionary<string, string> _endpoints;
    private readonly SmsClubSenderOptions _options;
    private readonly IDelay _delay;

    public SmsClubSender(
        HttpClient httpClient,
        IReadOnlyDictionary<string, string> endpoints,
        SmsClubSenderOptions options,
        IDelay delay
    )
    {
        _delay = delay.ThrowIfNull();
        _options = options.ThrowIfNull();
        _endpoints = new Dictionary<string, string>(endpoints);
        _httpClient = httpClient.ThrowIfNull();
    }

    public async Task<SendSmsClubResponse> SendSmsAsync(SendSmsClubRequest clubRequest)
    {
        using var content = clubRequest.ToJsonHttpContent();
        var url = _endpoints[Constants.SmsSendEndpointId];
        using var httpResponseMessage = await _httpClient.PostAsync(url, content);
        httpResponseMessage.ThrowIfNotSuccess();

        return await httpResponseMessage.Content.ReadFromJsonAsync<SendSmsClubResponse>();
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

            if (currentCount == _options.Count)
            {
                currentCount = 0;
                await _delay.DelayAsync(_options.Wait);
            }
        }
    }
}
