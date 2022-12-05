namespace WorkTool.Core.Modules.Common.Extensions;

public static class StreamExtension
{
    public static TStream Reset<TStream>(this TStream stream) where TStream : Stream
    {
        stream.Position = 0;

        return stream;
    }

    public static byte[] ReadAll<TStream>(this TStream stream, ushort bufferSize)
        where TStream : Stream
    {
        var buffer = new byte[bufferSize];
        using var ms = new MemoryStream();
        int read;

        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            ms.Write(buffer, 0, read);
        }

        return ms.ToArray();
    }

    public static byte[] ReadAll<TStream>(this TStream stream) where TStream : Stream
    {
        using var ms = new MemoryStream();
        stream.CopyTo(ms);

        return ms.ToArray();
    }

    public static async Task<byte[]> ReadAllAsync<TStream>(this TStream stream, ushort bufferSize)
        where TStream : Stream
    {
        var buffer = new byte[bufferSize];
        await using var ms = new MemoryStream();
        int read;

        while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await ms.WriteAsync(buffer, 0, read);
        }

        return ms.ToArray();
    }

    public static async Task<byte[]> ReadAllAsync<TStream>(this TStream stream)
        where TStream : Stream
    {
        await using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);

        return ms.ToArray();
    }

    public static Task<string> ToStringValue<TStream>(this TStream stream) where TStream : Stream
    {
        return stream.ToStringValue(Encoding.UTF8);
    }

    public static Task<string> ToStringValue<TStream>(this TStream stream, Encoding encoding)
        where TStream : Stream
    {
        var reader = new StreamReader(stream, encoding);

        return reader.ReadToEndAsync();
    }
}
