namespace WorkTool.Core.Applications.LanguageTeacher.Services;

public class LanguageDictionaryBuilder<TLanguage, TTranslate> : IBuilder<LanguageDictionary<TLanguage, TTranslate>>
    where TLanguage : notnull
{
    private readonly Dictionary<TLanguage, List<TTranslate>> _translates;

    public LanguageDictionary<TLanguage, TTranslate> Build()
    {
        var dictionary = new Dictionary<TLanguage, TranslateEnumerable<TTranslate>>();

        foreach (var translate in _translates)
        {
            var translateEnumerable = new TranslateEnumerable<TTranslate>(translate.Value);
            dictionary.Add(translate.Key, translateEnumerable);
        }

        return new LanguageDictionary<TLanguage, TTranslate>(dictionary);
    }

    public LanguageDictionaryBuilder<TLanguage, TTranslate> Add(TLanguage englishWord, TTranslate russianWord)
    {
        if (!_translates.ContainsKey(englishWord))
        {
            _translates[englishWord] = new List<TTranslate>();
        }

        _translates[englishWord].Add(russianWord);

        return this;
    }
}