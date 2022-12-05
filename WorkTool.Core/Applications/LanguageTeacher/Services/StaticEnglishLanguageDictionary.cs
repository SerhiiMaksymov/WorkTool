namespace WorkTool.Core.Applications.LanguageTeacher.Services;

public class StaticEnglishLanguageDictionary : ILanguageDictionary<EnglishWord, RussianWord>
{
    private readonly Dictionary<EnglishWord, TranslateEnumerable<RussianWord>> _source;

    public StaticEnglishLanguageDictionary()
    {
        _source = new Dictionary<EnglishWord, TranslateEnumerable<RussianWord>>
        {
            { new EnglishWord(), new TranslateEnumerable<RussianWord>(new[] { new RussianWord() }) }
        };
    }

    public TranslateEnumerable<RussianWord> this[EnglishWord key] => _source[key];

    public IEnumerable<EnglishWord> Keys => _source.Keys;
    public IEnumerable<TranslateEnumerable<RussianWord>> Values => _source.Values;
    public int Count => _source.Count;

    public IEnumerator<KeyValuePair<EnglishWord, TranslateEnumerable<RussianWord>>> GetEnumerator()
    {
        return _source.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _source.GetEnumerator();
    }

    public bool ContainsKey(EnglishWord key)
    {
        return _source.ContainsKey(key);
    }

    public bool TryGetValue(EnglishWord key, out TranslateEnumerable<RussianWord> value)
    {
        return _source.TryGetValue(key, out value);
    }
}
