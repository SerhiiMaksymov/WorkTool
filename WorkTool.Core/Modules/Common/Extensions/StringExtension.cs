namespace WorkTool.Core.Modules.Common.Extensions;

public static class StringExtension
{
    public static string ThrowIfNullOrWhiteSpace(this                              string str,
                                                 [CallerArgumentExpression("str")] string paramName = "")
    {
        str.ThrowIfNull(paramName);

        if (str.IsNullOrWhiteSpace())
        {
            throw new WhiteSpaceException(paramName);
        }

        return str;
    }

    public static bool IsNullOrWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    public static FileInfo ToFile(this string path)
    {
        return new FileInfo(path);
    }

    public static DirectoryInfo ToDirectory(this string path)
    {
        return new DirectoryInfo(path);
    }

    public static string ToConsoleLine(this string line)
    {
        Console.WriteLine(line);

        return line;
    }

    public static async Task<MemoryStream> ToStreamAsync(this string str, Encoding encoding)
    {
        var bytes  = str.ToByteArray(encoding);
        var stream = new MemoryStream();
        await stream.WriteAsync(bytes);

        return stream;
    }

    public static Task<MemoryStream> ToStreamAsync(this string str)
    {
        return str.ToStreamAsync(Encoding.UTF8);
    }

    public static byte[] ToByteArray(this string str, Encoding encoding)
    {
        return encoding.GetBytes(str);
    }

    public static byte[] ToByteArray(this string str)
    {
        return str.ToByteArray(Encoding.UTF8);
    }

    public static string ToWriteToFile(this string str, FileInfo file, Encoding encoding)
    {
        if (file.Exists)
        {
            file.Delete();
        }

        using var stream = file.Create();
        var       bytes  = encoding.GetBytes(str);
        stream.Write(bytes);

        return str;
    }

    public static string ToWriteToFile(this string str, FileInfo file)
    {
        return str.ToWriteToFile(file, Encoding.UTF8);
    }
}