namespace WorkTool.Core.Modules.SmsClub.Helpers;

public static class Constants
{
    public static readonly Uri DefaultHost = new("https://im.smsclub.mobi/");
    public const string SmsSendEndpointId = "SendSms";
    public const string DefaultSmsSendEndpointValue = "sms/send";
    public static readonly IReadOnlyDictionary<string, string> DefaultEndpoints = new Dictionary<
        string,
        string
    >()
    {
        { SmsSendEndpointId, DefaultSmsSendEndpointValue }
    };
}
