namespace WorkTool.Core.Modules.Common.Services;

public class RandomStringGuid : IRandom<string>
{
    public RandomStringGuid(string format)
    {
        Format = format.ThrowIfNullOrWhiteSpace();
    }

    public string Format { get; }

    public string GetRandom()
    {
        return Guid.NewGuid().ToString(Format);
    }
}
