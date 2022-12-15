using Constants = WorkTool.Core.Modules.Common.Helpers.Constants;

namespace WorkTool.Core.Modules.Common.Services;

public class RandomUInt16 : IRandom<ushort, Interval<ushort>>
{
    public ushort GetRandom(Interval<ushort> options)
    {
        var value = Constants.Random.Next(options.Min, options.Max + 1);

        return (ushort)value;
    }
}
