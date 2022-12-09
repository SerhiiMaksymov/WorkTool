namespace WorkTool.Core.Modules.SmsClub.Helpers;

public static class SmsClubSender
{
    public const string DefaultHostString = "https://im.smsclub.mobi/";

    public const string DefaultSmsSendEndpointString =
        DefaultHostString + SmsSenderEndpoints.DefaultSmsSendEndpoint;

    public const string DefaultSmsOriginatorEndpointString =
        DefaultHostString + SmsSenderEndpoints.DefaultSmsOriginatorEndpoint;

    public const string DefaultSmsStatusEndpointString =
        DefaultHostString + SmsSenderEndpoints.DefaultSmsStatusEndpoint;

    public static readonly Uri DefaultHostUri = new(DefaultHostString);
}
