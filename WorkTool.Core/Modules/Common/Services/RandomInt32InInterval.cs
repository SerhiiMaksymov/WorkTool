namespace WorkTool.Core.Modules.Common.Services;

public class RandomInt32InInterval : IRandom<int, Interval<int>>
{
    public static readonly RandomInt32InInterval Default = new ();

    public int GetRandom(Interval<int> options)
    {
        var value = CommonConstants.Random.Next(options.Min, options.Max + 1);

        return value;
    }
}