namespace WorkTool.SourceGenerator.Helpers;

public static class Consts
{
    public const string EnumValueMember        = "value__";
    public const string EnumCtorMember         = ".ctor";
    public const string EnumerableTypeFullName = "System.Collections.Generic.IEnumerable";
    public const string CollectionTypeFullName = "System.Collections.Generic.ICollection";

    public static readonly IEnumerable<string> EnumerableNames = new[]
    {
        EnumerableTypeFullName
    };

    public static readonly IEnumerable<string> CollectionNames = new[]
    {
        CollectionTypeFullName
    };

    public static readonly IEnumerable<string> EnumDefaultMembers = new[]
    {
        EnumValueMember,
        EnumCtorMember
    };
}