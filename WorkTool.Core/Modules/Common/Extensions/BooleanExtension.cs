namespace WorkTool.Core.Modules.Common.Extensions;

public static class BooleanExtension
{
    public static bool IsTrue(this bool? b)
    {
        return b == true;
    }

    public static bool IsTrue(this bool b)
    {
        return b;
    }

    public static bool IsFalse(this bool b)
    {
        return !b;
    }

    public static bool IsFalse(this bool? b)
    {
        return b == false;
    }

    public static bool IsNullOrFalse(this bool? b)
    {
        return b is null || b.IsFalse();
    }
}
