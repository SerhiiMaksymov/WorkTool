namespace WorkTool.Core.Modules.SmsClub.Models;

public readonly struct MessageItem<TParameters> where TParameters : notnull
{
    public readonly Message Message;
    public readonly TParameters Parameters;

    public MessageItem(Message message, TParameters parameters)
    {
        Message = message;
        Parameters = parameters.ThrowIfNull();
    }
}
