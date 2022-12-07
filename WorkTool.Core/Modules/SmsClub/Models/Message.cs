namespace WorkTool.Core.Modules.SmsClub.Models;

public readonly struct Message
{
    public readonly string RecipientId;
    public readonly string RecipientName;
    public readonly string Template;

    public Message(string recipientName, string template, string recipientId)
    {
        RecipientId = recipientId.ThrowIfNullOrWhiteSpace();
        RecipientName = recipientName.ThrowIfNullOrWhiteSpace();
        Template = template.ThrowIfNullOrWhiteSpace();
    }
}
