namespace WorkTool.Core.Modules.SmsClub.Interfaces;

public interface ISmsClubSender<TParameters> where TParameters : notnull
{
    Task<SmsResponse<Balance>> GetBalanceAsync();

    Task<SmsResponse<ArraySuccessRequest>> GetOriginatorsAsync();

    Task<SmsResponse<DictionarySuccessRequest>> GetSmsStatusAsync(IEnumerable<string> smsIds);

    Task<SmsResponse<DictionarySuccessRequest>> SendSmsAsync(SendSmsRequest request);

    IAsyncEnumerable<SmsResponse<DictionarySuccessRequest>> SendsSmsesAsync(
        MessageItemsCollection<TParameters> messageItems
    );
}
