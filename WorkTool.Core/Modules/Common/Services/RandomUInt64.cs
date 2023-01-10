namespace WorkTool.Core.Modules.Common.Services;

public class RandomUInt64 : IRandom<ulong>
{
    private readonly IRandom<ulong, Interval<ulong>> random;
    private readonly Interval<ulong> interval;

    public RandomUInt64(IRandom<ulong, Interval<ulong>> random, Interval<ulong> interval)
    {
        this.random = random;
        this.interval = interval;
    }

    public ulong GetRandom()
    {
        return random.GetRandom(interval);
    }
}
