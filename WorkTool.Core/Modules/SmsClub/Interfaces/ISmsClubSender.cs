namespace WorkTool.Core.Modules.SmsClub.Interfaces;

public interface ISmsClubSender<TParameters> where TParameters : notnull
{
    Task<BalanceResponse> GetBalanceAsync();

    Task<ArraySmsResponse> GetOriginatorsAsync();

    Task<DictionarySmsResponse> GetSmsStatusAsync(IEnumerable<string> smsIds);

    Task<DictionarySmsResponse> SendSmsAsync(SendSmsRequest request);

    IAsyncEnumerable<DictionarySmsResponse> SendsSmsesAsync(
        MessageItemsCollection<TParameters> messageItems
    );
}
