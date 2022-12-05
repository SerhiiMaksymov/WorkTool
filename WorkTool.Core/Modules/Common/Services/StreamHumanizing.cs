namespace WorkTool.Core.Modules.Common.Services;

public class StreamHumanizing : IHumanizing<Stream, string>
{
    private readonly string _breakString;
    private readonly ulong _charSize;
    private readonly Encoding _encoding;
    private readonly ushort _encodingCharSize;
    private readonly string _splitString;

    public StreamHumanizing(
        string splitString,
        ulong charSize,
        string breakStrings,
        Encoding encoding,
        ushort encodingCharSize
    )
    {
        _splitString = splitString;
        _charSize = charSize;
        _breakString = breakStrings;
        _encoding = encoding.ThrowIfNull();
        _encodingCharSize = encodingCharSize;
    }

    public StreamHumanizing() : this(" ", 2, Environment.NewLine, Encoding.UTF8, 4) { }

    public string Humanize(Stream stream)
    {
        var stringBuilder = new StringBuilder();
        var buffer = new Span<byte>(new byte[_encodingCharSize * _charSize]);

        while (stream.Read(buffer) != 0)
        {
            for (var index = 0; index < buffer.Length; index++)
            {
                stringBuilder.Append(buffer[index].ToString("X2"));
                stringBuilder.Append(_splitString);
            }

            stringBuilder.Append(_encoding.GetString(buffer));
            stringBuilder.Append(_breakString);
        }

        return stringBuilder.ToString();
    }
}
