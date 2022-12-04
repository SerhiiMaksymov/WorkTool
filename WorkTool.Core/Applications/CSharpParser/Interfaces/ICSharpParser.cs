namespace WorkTool.Core.Applications.CSharpParser.Interfaces;

public interface ICSharpParser
{
    IAsyncEnumerable<ICSharpToken> ParseAsync(Stream stream);
}