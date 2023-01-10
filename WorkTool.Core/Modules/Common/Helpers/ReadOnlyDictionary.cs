namespace WorkTool.Core.Modules.Common.Helpers;

public static class ReadOnlyDictionary<TKey, TValue> where TKey : notnull
{
    public static readonly IReadOnlyDictionary<TKey, TValue> Empty = new Dictionary<TKey, TValue>();
}
