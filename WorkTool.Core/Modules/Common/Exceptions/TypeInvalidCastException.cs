namespace WorkTool.Core.Modules.Common.Exceptions;

public class TypeInvalidCastException : InvalidCastException
{
    public TypeInvalidCastException(Type expectedType, Type currentType)
        : base(CreateMessage(expectedType, currentType))
    {
        CurrentType = currentType;
        ExpectedTypes = new[] { expectedType };
    }

    public TypeInvalidCastException(Type[] expectedTypes, Type currentType)
        : base(CreateMessage(expectedTypes, currentType))
    {
        CurrentType = currentType;
        ExpectedTypes = expectedTypes.ThrowIfNullOrEmpty().ToArray();
    }

    public TypeInvalidCastException(IEnumerable<Type> expectedTypes, Type currentType)
        : this(expectedTypes.ToArray(), currentType) { }

    public Type CurrentType { get; }
    public IEnumerable<Type> ExpectedTypes { get; }

    private static string CreateMessage(Type expectedType, Type currentType)
    {
        return $"Expected type \"{expectedType}\" actual type \"{currentType}\".";
    }

    private static string CreateMessage(IEnumerable<Type> expectedTypes, Type currentType)
    {
        return $"Expected type \"{expectedTypes.JoinString("\", \"")}\" actual type \"{currentType}\".";
    }
}
