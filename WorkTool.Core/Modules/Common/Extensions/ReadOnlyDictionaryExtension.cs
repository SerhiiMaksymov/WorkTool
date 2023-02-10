namespace WorkTool.Core.Modules.Common.Extensions;

public static class ReadOnlyDictionaryExtension
{
    public static TValue Get<TKey, TValue>(this IReadOnlyDictionary<TKey,TValue> dictionary, TKey key, TValue def)
    {
        if (dictionary.TryGetValue(key, out var result))
        {
          return  result;
        }

        return def;
    }
}