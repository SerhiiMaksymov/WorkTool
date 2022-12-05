namespace WorkTool.Core.Applications.CSharpParser.Services;

public class CSharpParserService : ICSharpParser
{
    private readonly ushort _bufferSize;

    public CSharpParserService(ushort bufferSize)
    {
        _bufferSize = bufferSize;
    }

    public async IAsyncEnumerable<ICSharpToken> ParseAsync(Stream stream)
    {
        var readCount = 0;
        var readBuffer = new byte[_bufferSize].AsMemory();

        do
        {
            var currentIndex = 0;
            readCount = await stream.ReadAsync(readBuffer);
            var indexes = GetWordIndexes(readBuffer);

            foreach (var index in indexes)
            {
                Encoding.UTF8
                    .GetString(readBuffer.Slice((int)index.Min, (int)(index.Max - index.Min)).Span)
                    .ToConsoleLine();
            }
        } while (readCount == _bufferSize);

        yield break;
    }

    private IEnumerable<Interval<uint>> GetWordIndexes(Memory<byte> buffer)
    {
        var currentIndex = 0;
        var start = 0;
        var end = 0;

        while (true)
        {
            var t = CommonMemoryConstants.WhiteSpace.ToArray();
            var index = buffer.IndexOf(CommonMemoryConstants.WhiteSpace, currentIndex);

            if (index.Index == -1)
            {
                break;
            }

            if (index.Index == currentIndex)
            {
                currentIndex += index.Match.Length;
                start = currentIndex;

                continue;
            }

            end = index.Index;

            yield return new Interval<uint>((uint)start, (uint)end);

            currentIndex = index.Index + index.Match.Length;
            start = currentIndex;
        }
    }
}
