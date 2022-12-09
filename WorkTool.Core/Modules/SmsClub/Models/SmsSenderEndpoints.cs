namespace WorkTool.Core.Modules.SmsClub.Models;

public readonly struct SmsSenderEndpoints
{
    public const string DefaultSmsSendEndpoint = "sms/send";
    public const string DefaultSmsStatusEndpoint = "sms/status";
    public const string DefaultSmsOriginatorEndpoint = "sms/originator";
    public static readonly SmsSenderEndpoints Default =
        new(DefaultSmsSendEndpoint, DefaultSmsStatusEndpoint, DefaultSmsOriginatorEndpoint);
    public readonly string SmsSendEndpoint;
    public readonly string SmsStatusEndpoint;
    public readonly string SmsOriginatorEndpoint;

    public SmsSenderEndpoints(
        string smsSendEndpoint,
        string smsStatusEndpoint,
        string smsOriginatorEndpoint
    )
    {
        SmsSendEndpoint = smsSendEndpoint.ThrowIfNullOrWhiteSpace();
        SmsStatusEndpoint = smsStatusEndpoint.ThrowIfNullOrWhiteSpace();
        SmsOriginatorEndpoint = smsOriginatorEndpoint.ThrowIfNullOrWhiteSpace();
    }
}
