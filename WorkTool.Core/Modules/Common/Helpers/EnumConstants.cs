namespace WorkTool.Core.Modules.Common.Helpers;

public class EnumConstants<TEnum> where TEnum : struct, Enum
{
    public static readonly IEnumerable<TEnum> Values;
    public static readonly IEnumerable<TEnum?> NullableValues;

    static EnumConstants()
    {
        Values = Enum.GetValues<TEnum>();
        NullableValues = Values.Select(x => (TEnum?)x).ToArray();
    }
}
