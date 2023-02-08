namespace WorkTool.Core.Modules.Common.Extensions;

public static class ReadOnlyMemoryExtension
{
    public static bool IsSingleValue<T>(this ReadOnlyMemory<T> memory)
    {
        if (memory.Length == 1)
        {
            return true;
        }

        return false;
    }

    public static T GetSingle<T>(this ReadOnlyMemory<T> memory)
    {
        if (memory.Length == 1)
        {
            return memory.Span[0];
        }

        throw new Exception(memory.Length.ToString());
    }

    public static bool TryGetSingleValue<T>(
        this ReadOnlyMemory<T> memory,
        [MaybeNullWhen(false)] out T value
    )
    {
        if (memory.IsSingleValue())
        {
            value = memory.Span[0];

            return true;
        }

        value = default;

        return false;
    }
}
