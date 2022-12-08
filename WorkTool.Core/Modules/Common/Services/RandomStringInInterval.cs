namespace WorkTool.Core.Modules.Common.Services;

public class RandomStringInInterval : IRandom<string, Interval<int>>
{
    private readonly IRandomArrayItem<char> randomArrayItemChar;
    private readonly IRandom<int, Interval<int>> randomInt32;
    private readonly char[] values;

    public RandomStringInInterval(
        string values,
        IRandomArrayItem<char> randomArrayItemChar,
        IRandom<int, Interval<int>> randomInt32
    )
    {
        this.randomArrayItemChar = randomArrayItemChar.ThrowIfNull();
        this.values = values.ThrowIfNull().ToArray();
        this.randomInt32 = randomInt32.ThrowIfNull();
    }

    public string? GetRandom(Interval<int> interval)
    {
        var value = randomInt32.GetRandom(interval);

        if (value == 0)
        {
            return string.Empty;
        }

        if (value == -1)
        {
            return null;
        }

        var result = new char[value];

        for (var index = 0; index < value; index++)
        {
            var @char = randomArrayItemChar.GetRandom(values);
            result[index] = @char;
        }

        return new string(result);
    }
}
