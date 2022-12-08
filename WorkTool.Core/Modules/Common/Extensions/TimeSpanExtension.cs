namespace WorkTool.Core.Modules.Common.Extensions;

public static class TimeSpanExtension
{
    public static Task Delay(this TimeSpan timeSpan)
    {
        return Task.Delay(timeSpan);
    }
}
