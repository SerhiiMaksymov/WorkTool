namespace WorkTool.Core.Modules.SmsClub.Extensions;

public static class MessageExtension
{
    public static string GetMessageValue<TParameters>(this Message message, TParameters parameters)
    {
        return message.Template.FormatWith(parameters);
    }
}
