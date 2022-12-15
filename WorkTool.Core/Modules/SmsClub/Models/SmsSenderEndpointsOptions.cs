namespace WorkTool.Core.Modules.SmsClub.Models;

public class SmsSenderEndpointsOptions
{
    public const string DefaultSmsSendEndpoint = "sms/send";
    public const string DefaultSmsStatusEndpoint = "sms/status";
    public const string DefaultSmsOriginatorEndpoint = "sms/originator";
    public const string DefaultSmsBalanceEndpoint = "sms/balance";

    public static readonly SmsSenderEndpointsOptions Default = new();

    public string SmsSend { get; set; } = DefaultSmsSendEndpoint;
    public string SmsStatus { get; set; } = DefaultSmsStatusEndpoint;
    public string SmsOriginator { get; set; } = DefaultSmsOriginatorEndpoint;
    public string SmsBalance { get; set; } = DefaultSmsBalanceEndpoint;
}
