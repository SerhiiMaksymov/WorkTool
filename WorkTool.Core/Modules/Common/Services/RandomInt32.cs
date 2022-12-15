using Constants = WorkTool.Core.Modules.Common.Helpers.Constants;

namespace WorkTool.Core.Modules.Common.Services;

public class RandomInt32 : IRandom<int>
{
    private readonly Interval<int> interval;

    public RandomInt32(Interval<int> interval)
    {
        this.interval = interval;
    }

    public int GetRandom()
    {
        var value = Constants.Random.Next(interval.Min, interval.Max + 1);

        return value;
    }
}
