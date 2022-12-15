namespace WorkTool.Core.Modules.SmsClub.Excpetions;

public class NotFoundSmsesException : Exception
{
    public NotFoundSmsesException(IEnumerable<string> smsIds) : this(smsIds.ToArray()) { }

    private NotFoundSmsesException(string[] smsIds) : base(CreateMassage(smsIds))
    {
        SmsIds = smsIds;
    }

    public IEnumerable<string> SmsIds { get; }

    private static string CreateMassage(string[] smsIds)
    {
        return smsIds.Length == 1
            ? $"Not found sms with id {smsIds[0]}."
            : $"Not found sms with ids {smsIds.JoinString(", ")}.";
    }
}
