namespace WorkTool.Core.Modules.SmsClub.Extensions;

public static class MessageItemExtension
{
    public static string GetMessageValue<TParameters>(this MessageItem<TParameters> messageItem)
        where TParameters : notnull
    {
        return messageItem.Message.GetMessageValue(messageItem.Parameters);
    }
}
