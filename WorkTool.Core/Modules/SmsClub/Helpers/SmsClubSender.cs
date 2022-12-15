namespace WorkTool.Core.Modules.SmsClub.Helpers;

public static class SmsClubSender
{
    public const string DefaultHostString = "https://im.smsclub.mobi/";
    public const string EnvironmentApiKeyName = "SMS_CLUB_API_KEY";
    public const string ConfigPath = "SmsClub";
    public const string ConfigHostPath = "SmsClub::Host";
    public const string ConfigOptionsPath = "SmsClub::Options";
    public const string ConfigEndpointsPath = "SmsClub::Endpoints";

    public const string DefaultSmsSendEndpointString =
        DefaultHostString + SmsSenderEndpointsOptions.DefaultSmsSendEndpoint;

    public const string DefaultSmsOriginatorEndpointString =
        DefaultHostString + SmsSenderEndpointsOptions.DefaultSmsOriginatorEndpoint;

    public const string DefaultSmsStatusEndpointString =
        DefaultHostString + SmsSenderEndpointsOptions.DefaultSmsStatusEndpoint;

    public static readonly Uri DefaultHostUri = new(DefaultHostString);
}
