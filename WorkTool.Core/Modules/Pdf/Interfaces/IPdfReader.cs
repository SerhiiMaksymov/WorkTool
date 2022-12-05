namespace WorkTool.Core.Modules.Pdf.Interfaces;

public interface IPdfReader
{
    IAsyncEnumerable<IPdfItem> ReadAsync(Stream stream);
}
