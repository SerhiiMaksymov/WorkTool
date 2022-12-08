namespace WorkTool.Core.Modules.SmsClub.Models;

public readonly struct SmsClubSenderEndpoints
{
    public const string DefaultSmsSendEndpoint = "sms/send";
    public const string DefaultSmsStatusEndpoint = "sms/status";
    public static readonly SmsClubSenderEndpoints Default =
        new(DefaultSmsSendEndpoint, DefaultSmsStatusEndpoint);
    public readonly string SmsSendEndpoint;
    public readonly string SmsStatusEndpoint;

    public SmsClubSenderEndpoints(string smsSendEndpoint, string smsStatusEndpoint)
    {
        SmsSendEndpoint = smsSendEndpoint.ThrowIfNullOrWhiteSpace();
        SmsStatusEndpoint = smsStatusEndpoint.ThrowIfNullOrWhiteSpace();
    }
}
