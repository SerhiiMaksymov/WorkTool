namespace WorkTool.Core.Applications.LanguageTeacher.Models;

public class TranslateEnumerable<TWord> : IEnumerable<TWord>
{
    private readonly List<TWord> _source;

    public TranslateEnumerable(IEnumerable<TWord> source)
    {
        _source = new List<TWord>(source);
    }

    public IEnumerator<TWord> GetEnumerator()
    {
        return _source.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _source.GetEnumerator();
    }
}