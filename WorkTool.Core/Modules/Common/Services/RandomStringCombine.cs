namespace WorkTool.Core.Modules.Common.Services;

public class RandomStringCombine : IRandom<string>
{
    private readonly List<IRandom<string>> randoms;

    public RandomStringCombine(IEnumerable<IRandom<string>> randoms)
    {
        this.randoms = new List<IRandom<string>>(randoms.ThrowIfNull());
    }

    public string GetRandom()
    {
        var builder = new StringBuilder();

        foreach (var random in randoms)
        {
            var value = random.GetRandom();
            builder.Append(value);
        }

        return builder.ToString();
    }
}