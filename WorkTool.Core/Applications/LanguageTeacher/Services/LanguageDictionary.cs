namespace WorkTool.Core.Applications.LanguageTeacher.Services;

public class LanguageDictionary<TLanguage, TTranslate> : ILanguageDictionary<TLanguage, TTranslate>
    where TLanguage : notnull
{
    private readonly Dictionary<TLanguage, TranslateEnumerable<TTranslate>> _source;

    public LanguageDictionary(IReadOnlyDictionary<TLanguage, TranslateEnumerable<TTranslate>> source)
    {
        _source = new Dictionary<TLanguage, TranslateEnumerable<TTranslate>>(source);
    }

    public TranslateEnumerable<TTranslate> this[TLanguage key] => _source[key];

    public IEnumerable<TLanguage>                       Keys   => _source.Keys;
    public IEnumerable<TranslateEnumerable<TTranslate>> Values => _source.Values;
    public int                                          Count  => _source.Count;

    public IEnumerator<KeyValuePair<TLanguage, TranslateEnumerable<TTranslate>>> GetEnumerator()
    {
        return _source.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _source.GetEnumerator();
    }

    public bool ContainsKey(TLanguage key)
    {
        return _source.ContainsKey(key);
    }

    public bool TryGetValue(TLanguage key, out TranslateEnumerable<TTranslate> value)
    {
        return _source.TryGetValue(key, out value);
    }
}