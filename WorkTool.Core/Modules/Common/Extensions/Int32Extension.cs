namespace WorkTool.Core.Modules.Common.Extensions;

public static class Int32Extension
{
    public static TimeSpan ToTimeSpanSeconds(this int value)
    {
        return TimeSpan.FromSeconds(value);
    }
}
