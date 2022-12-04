namespace WorkTool.Core.Modules.Common.Extensions;

public static class ByteExtension
{
    public static string ToStringValue(this byte[] array, Encoding encoding)
    {
        return encoding.GetString(array);
    }

    public static string ToStringValue(this byte[] array)
    {
        return array.ToStringValue(Encoding.UTF8);
    }

    public static int IndexOf(this byte[] haystack, byte[] needle)
    {
        return haystack.IndexOf(needle, 0);
    }

    public static int IndexOf(this byte[] haystack, byte[] needle, int start)
    {
        for (var i = start; i <= haystack.Length - needle.Length; i++)
        {
            if (haystack.Match(needle, i))
            {
                return i;
            }
        }

        return -1;
    }

    public static bool Match(this byte[] haystack, byte[] needle, int start)
    {
        if (needle.Length + start > haystack.Length)
        {
            return false;
        }

        for (var i = 0; i < needle.Length; i++)
        {
            if (needle[i] != haystack[i + start])
            {
                return false;
            }
        }

        return true;
    }

    public static int IndexOf(this Memory<byte> haystack, ReadOnlyMemory<byte> needle)
    {
        return haystack.IndexOf(needle, 0);
    }

    public static IndexOfNeedlesResult IndexOf(
    this Memory<byte>                    haystack,
    ReadOnlyMemory<ReadOnlyMemory<byte>> needles,
    int                                  start)
    {
        var result = new IndexOfNeedlesResult();

        foreach (var needle in needles.Span)
        {
            var index = haystack.IndexOf(needle, start);

            if (index == -1)
            {
                continue;
            }

            if (result.Index == -1)
            {
                result = new IndexOfNeedlesResult(index, needle);
            }
            else if (index < result.Index)
            {
                result = new IndexOfNeedlesResult(index, needle);
            }
            else if (index == result.Index && result.Match.Length < needle.Length)
            {
                result = new IndexOfNeedlesResult(index, needle);
            }
        }

        return result;
    }

    public static IndexOfNeedlesResult IndexOf(this Memory<byte> haystack, ReadOnlyMemory<ReadOnlyMemory<byte>> needles)
    {
        return haystack.IndexOf(needles, 0);
    }

    public static int IndexOf(this Memory<byte> haystack, ReadOnlyMemory<byte> needle, int start)
    {
        for (var i = start; i <= haystack.Length - needle.Length; i++)
        {
            if (haystack.Match(needle, i))
            {
                return i;
            }
        }

        return -1;
    }

    public static bool Match(this Memory<byte> haystack, ReadOnlyMemory<byte> needle, int start)
    {
        if (needle.Length + start > haystack.Length)
        {
            return false;
        }

        for (var i = 0; i < needle.Length; i++)
        {
            if (needle.Span[i] != haystack.Span[i + start])
            {
                return false;
            }
        }

        return true;
    }

    public static int IndexOf(this Memory<byte> haystack, Memory<byte> needle)
    {
        return haystack.IndexOf(needle, 0);
    }

    public static int IndexOf(this Memory<byte> haystack, Memory<byte> needle, int start)
    {
        for (var i = start; i <= haystack.Length - needle.Length; i++)
        {
            if (haystack.Match(needle, i))
            {
                return i;
            }
        }

        return -1;
    }

    public static bool Match(this Memory<byte> haystack, Memory<byte> needle, int start)
    {
        if (needle.Length + start > haystack.Length)
        {
            return false;
        }

        for (var i = 0; i < needle.Length; i++)
        {
            if (needle.Span[i] != haystack.Span[i + start])
            {
                return false;
            }
        }

        return true;
    }

    public static int IndexOf(this Memory<byte> haystack, byte[] needle)
    {
        return haystack.IndexOf(needle, 0);
    }

    public static int IndexOf(this Memory<byte> haystack, byte[] needle, int start)
    {
        for (var i = start; i <= haystack.Length - needle.Length; i++)
        {
            if (haystack.Match(needle, i))
            {
                return i;
            }
        }

        return -1;
    }

    public static bool Match(this Memory<byte> haystack, byte[] needle, int start)
    {
        if (needle.Length + start > haystack.Length)
        {
            return false;
        }

        for (var i = 0; i < needle.Length; i++)
        {
            if (needle[i] != haystack.Span[i + start])
            {
                return false;
            }
        }

        return true;
    }

    public static int IndexOf(this Span<byte> haystack, byte[] needle)
    {
        return haystack.IndexOf(needle, 0);
    }

    public static int IndexOf(this Span<byte> haystack, byte[] needle, int start)
    {
        for (var i = start; i <= haystack.Length - needle.Length; i++)
        {
            if (haystack.Match(needle, i))
            {
                return i;
            }
        }

        return -1;
    }

    public static bool Match(this Span<byte> haystack, byte[] needle, int start)
    {
        if (needle.Length + start > haystack.Length)
        {
            return false;
        }

        for (var i = 0; i < needle.Length; i++)
        {
            if (needle[i] != haystack[i + start])
            {
                return false;
            }
        }

        return true;
    }
}