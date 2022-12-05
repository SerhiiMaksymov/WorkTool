namespace WorkTool.Core.Modules.Common.Services;

public class RandomInt32 : IRandom<int>
{
    private readonly Interval<int> _interval;

    public RandomInt32(Interval<int> interval)
    {
        _interval = interval;
    }

    public int GetRandom()
    {
        var value = CommonConstants.Random.Next(_interval.Min, _interval.Max + 1);

        return value;
    }
}
