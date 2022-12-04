namespace WorkTool.Core.Modules.Pdf.Services;

public class PdfReaderService : IPdfReader
{
    private readonly ushort _bufferSize;

    public PdfReaderService(ushort bufferSize)
    {
        _bufferSize = bufferSize.ThrowIfNull();
    }

    public async IAsyncEnumerable<IPdfItem> ReadAsync(Stream stream)
    {
        var readBuffer = new byte[_bufferSize].AsMemory();
        await stream.ReadAsync(readBuffer);

        var index = readBuffer.IndexOf(
            new byte[]
            {
                10
            });

        yield break;
    }
}