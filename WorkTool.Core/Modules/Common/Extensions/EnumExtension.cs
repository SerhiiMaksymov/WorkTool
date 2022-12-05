namespace WorkTool.Core.Modules.Common.Extensions;

public static class EnumExtension
{
    public static unsafe bool WorkToolHasAllFlags<T>(this T value, T flags)
        where T : unmanaged, Enum
    {
        if (sizeof(T) == 1)
        {
            var byteValue = Unsafe.As<T, byte>(ref value);
            var byteFlags = Unsafe.As<T, byte>(ref flags);

            return (byteValue & byteFlags) == byteFlags;
        }

        if (sizeof(T) == 2)
        {
            var shortValue = Unsafe.As<T, short>(ref value);
            var shortFlags = Unsafe.As<T, short>(ref flags);

            return (shortValue & shortFlags) == shortFlags;
        }

        if (sizeof(T) == 4)
        {
            var intValue = Unsafe.As<T, int>(ref value);
            var intFlags = Unsafe.As<T, int>(ref flags);

            return (intValue & intFlags) == intFlags;
        }

        if (sizeof(T) == 8)
        {
            var longValue = Unsafe.As<T, long>(ref value);
            var longFlags = Unsafe.As<T, long>(ref flags);

            return (longValue & longFlags) == longFlags;
        }

        throw new NotSupportedException(
            "Enum with size of " + Unsafe.SizeOf<T>() + " are not supported"
        );
    }
}
