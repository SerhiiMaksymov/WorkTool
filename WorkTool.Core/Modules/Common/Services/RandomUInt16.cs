namespace WorkTool.Core.Modules.Common.Services;

public class RandomUInt16 : IRandom<ushort>
{
    private readonly IRandom<ushort, Interval<ushort>> random;
    private readonly Interval<ushort> interval;

    public RandomUInt16(IRandom<ushort, Interval<ushort>> random, Interval<ushort> interval)
    {
        this.random = random;
        this.interval = interval;
    }

    public ushort GetRandom()
    {
        var value = random.GetRandom(interval);

        return value;
    }
}
