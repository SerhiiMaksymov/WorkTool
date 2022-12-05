namespace WorkTool.Core.Modules.Common.Models;

public readonly struct Interval<TValue>
{
    public readonly TValue Min;
    public readonly TValue Max;

    public Interval(TValue min, TValue max)
    {
        Min = min;
        Max = max;
    }
}
