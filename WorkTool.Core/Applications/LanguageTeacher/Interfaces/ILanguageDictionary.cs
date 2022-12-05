namespace WorkTool.Core.Applications.LanguageTeacher.Interfaces;

public interface ILanguageDictionary<TLanguage, TTranslate>
    : IReadOnlyDictionary<TLanguage, TranslateEnumerable<TTranslate>> { }
