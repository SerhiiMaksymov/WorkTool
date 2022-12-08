namespace WorkTool.Core.Modules.SmsClub.Models;

public readonly struct MessageItemsCollection<TParameters> : IEnumerable<MessageItem<TParameters>>
    where TParameters : notnull
{
    private readonly List<MessageItem<TParameters>> messages;

    public int Count => messages.Count;

    public MessageItemsCollection()
    {
        messages = new();
    }

    public MessageItemsCollection(IEnumerable<MessageItem<TParameters>> messages)
    {
        this.messages = new List<MessageItem<TParameters>>(messages);
    }

    public IEnumerator<MessageItem<TParameters>> GetEnumerator()
    {
        return messages.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
