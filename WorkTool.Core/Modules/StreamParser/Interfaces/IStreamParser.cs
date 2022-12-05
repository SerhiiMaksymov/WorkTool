namespace WorkTool.Core.Modules.StreamParser.Interfaces;

public interface IStreamParser<TToken, TValue> where TToken : class
{
    IEnumerable<TToken> Parse(IEnumerable<TValue> values);
}
