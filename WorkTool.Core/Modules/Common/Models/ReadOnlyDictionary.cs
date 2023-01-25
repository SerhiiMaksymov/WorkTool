namespace WorkTool.Core.Modules.Common.Models;

public readonly struct ReadOnlyDictionary<TKey, TValue> where TKey : notnull
{
    public readonly Memory<KeyValuePair<TKey, TValue>> Memory;
    public readonly int Count;
    public readonly Memory<TKey> Keys;
    public readonly Memory<TValue> Values;

    public ReadOnlyDictionary(IReadOnlyDictionary<TKey, TValue> dictionary)
        : this(dictionary.ToArray()) { }

    private ReadOnlyDictionary(Memory<KeyValuePair<TKey, TValue>> memory)
    {
        Memory = memory;
        Count = memory.Length;
        var keys = new TKey[memory.Length];
        var values = new TValue[memory.Length];

        for (var index = 0; index < memory.Length; index++)
        {
            keys[index] = memory.Span[index].Key;
            values[index] = memory.Span[index].Value;
        }

        Keys = keys;
        Values = values;
    }

    public static implicit operator ReadOnlyDictionary<TKey, TValue>(
        Dictionary<TKey, TValue> dictionary
    )
    {
        return new ReadOnlyDictionary<TKey, TValue>(dictionary);
    }

    public Enumerator GetEnumerator()
    {
        return new(Memory.Span);
    }

    public bool ContainsKey(TKey key)
    {
        foreach (var item in Memory.Span)
        {
            if (item.Equals(key))
            {
                return true;
            }
        }

        return false;
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        foreach (var item in Memory.Span)
        {
            if (item.Key.Equals(key))
            {
                value = item.Value;

                return true;
            }
        }

        value = default;

        return false;
    }

    public TValue this[TKey key] => throw new NotImplementedException();

    public ref struct Enumerator
    {
        private readonly Span<KeyValuePair<TKey, TValue>> span;
        private int index;

        public ref KeyValuePair<TKey, TValue> Current => ref span[index];

        internal Enumerator(Span<KeyValuePair<TKey, TValue>> span)
        {
            this.span = span;
            index = -1;
        }

        public bool MoveNext()
        {
            var num = index + 1;

            if (num >= span.Length)
                return false;

            index = num;

            return true;
        }
    }
}
