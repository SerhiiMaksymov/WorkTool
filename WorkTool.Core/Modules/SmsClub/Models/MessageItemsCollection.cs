namespace WorkTool.Core.Modules.SmsClub.Models;

public readonly struct MessageItemsCollection<TParameters> : IEnumerable<MessageItem<TParameters>>
    where TParameters : notnull
{
    private readonly List<MessageItem<TParameters>> _messages;

    public int Count => _messages.Count;

    public MessageItemsCollection()
    {
        _messages = new();
    }

    public MessageItemsCollection(IEnumerable<MessageItem<TParameters>> messages)
    {
        _messages = new List<MessageItem<TParameters>>(messages);
    }

    public IEnumerator<MessageItem<TParameters>> GetEnumerator()
    {
        return _messages.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
