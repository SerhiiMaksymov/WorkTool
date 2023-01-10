namespace WorkTool.Core.Modules.Common.Models;

public readonly record struct TypeIdentifier
{
    private readonly object identifier;

    public TypeIdentifier(IReflect type)
    {
        identifier = type.UnderlyingSystemType;
    }

    public static implicit operator TypeIdentifier(Type type)
    {
        return new(type);
    }
}
