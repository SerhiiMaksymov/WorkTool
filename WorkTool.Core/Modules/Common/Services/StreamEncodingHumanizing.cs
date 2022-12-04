namespace WorkTool.Core.Modules.Common.Services;

public class StreamEncodingHumanizing : IHumanizing<Stream, string>
{
    private readonly Encoding _encoding;

    public StreamEncodingHumanizing(Encoding encoding)
    {
        _encoding = encoding.ThrowIfNull();
    }

    public StreamEncodingHumanizing() : this(Encoding.UTF8)
    {
    }

    public string Humanize(Stream input)
    {
        var bytes  = input.ReadAll();
        var result = _encoding.GetString(bytes);

        return result;
    }
}