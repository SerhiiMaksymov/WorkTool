namespace WorkTool.Core.Modules.Common.Services;

public class RandomArrayItem<TValue> : IRandomArrayItem<TValue>
{
    public static readonly RandomArrayItem<TValue> IncludeDefault = new(true);
    public static readonly RandomArrayItem<TValue> ExcludeDefault = new(false);

    private readonly bool includeDefault;

    public RandomArrayItem(bool includeDefault)
    {
        this.includeDefault = includeDefault;
    }

    public TValue GetRandom(TValue[] values)
    {
        if (values.Length == 0)
        {
            return includeDefault ? default : throw new Exception("Empty array.");
        }

        var min = includeDefault ? -1 : 0;
        var index = CommonConstants.Random.Next(min, values.Length);

        if (index == -1)
        {
            return default;
        }

        return values[index];
    }
}
