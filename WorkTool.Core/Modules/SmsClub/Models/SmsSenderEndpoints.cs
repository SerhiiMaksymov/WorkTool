namespace WorkTool.Core.Modules.SmsClub.Models;

public readonly struct SmsSenderEndpoints
{
    public const string DefaultSmsSendEndpoint = "sms/send";
    public const string DefaultSmsStatusEndpoint = "sms/status";
    public const string DefaultSmsOriginatorEndpoint = "sms/originator";
    public const string DefaultSmsBalanceEndpoint = "sms/balance";

    public static readonly SmsSenderEndpoints Default =
        new(
            DefaultSmsSendEndpoint,
            DefaultSmsStatusEndpoint,
            DefaultSmsOriginatorEndpoint,
            DefaultSmsBalanceEndpoint
        );

    public readonly string SmsSendEndpoint;
    public readonly string SmsStatusEndpoint;
    public readonly string SmsOriginatorEndpoint;
    public readonly string SmsBalanceEndpoint;

    public SmsSenderEndpoints(
        string smsSendEndpoint,
        string smsStatusEndpoint,
        string smsOriginatorEndpoint,
        string smsBalanceEndpoint
    )
    {
        SmsBalanceEndpoint = smsBalanceEndpoint.ThrowIfNullOrWhiteSpace();
        SmsSendEndpoint = smsSendEndpoint.ThrowIfNullOrWhiteSpace();
        SmsStatusEndpoint = smsStatusEndpoint.ThrowIfNullOrWhiteSpace();
        SmsOriginatorEndpoint = smsOriginatorEndpoint.ThrowIfNullOrWhiteSpace();
    }
}
