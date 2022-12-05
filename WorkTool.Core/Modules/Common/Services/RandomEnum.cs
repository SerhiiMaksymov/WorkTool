namespace WorkTool.Core.Modules.Common.Services;

public class RandomEnum<TEnum> : IRandom<TEnum> where TEnum : struct, Enum
{
    public static readonly RandomEnum<TEnum> Default = new(RandomArrayItem<TEnum>.ExcludeDefault);
    private static readonly TEnum[] Values = EnumConstants<TEnum>.Values.ToArray();

    public readonly IRandomArrayItem<TEnum> RandomArrayItemEnum;

    public RandomEnum(IRandomArrayItem<TEnum> randomArrayItemEnum)
    {
        RandomArrayItemEnum = randomArrayItemEnum.ThrowIfNull();
    }

    public TEnum GetRandom()
    {
        var result = RandomArrayItemEnum.GetRandom(Values);

        return result;
    }
}

public class RandomNullableEnum<TEnum> : IRandom<TEnum?> where TEnum : struct, Enum
{
    public static readonly RandomNullableEnum<TEnum> Default =
        new(RandomArrayItem<TEnum?>.IncludeDefault);
    private static readonly TEnum?[] Values = EnumConstants<TEnum>.NullableValues.ToArray();

    public readonly IRandomArrayItem<TEnum?> RandomArrayItemEnum;

    public RandomNullableEnum(IRandomArrayItem<TEnum?> randomArrayItemEnum)
    {
        RandomArrayItemEnum = randomArrayItemEnum.ThrowIfNull();
    }

    public TEnum? GetRandom()
    {
        var result = RandomArrayItemEnum.GetRandom(Values);

        return result;
    }
}
