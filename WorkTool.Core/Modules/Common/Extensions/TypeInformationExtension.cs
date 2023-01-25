namespace WorkTool.Core.Modules.Common.Extensions;

public static class TypeInformationExtension
{
    public static bool IsEnumerable(this TypeInformation type)
    {
        if (!type.IsGenericType)
        {
            return false;
        }

        if (type.GenericTypeArguments.Length != 1)
        {
            return false;
        }

        if (type.Type.GetGenericTypeDefinition() != typeof(IEnumerable<>))
        {
            return false;
        }

        return true;
    }

    public static ConstructorInfo? GetSingleConstructor(this TypeInformation type)
    {
        if (type.Constructors.Length == 0)
        {
            return null;
        }

        if (type.Constructors.Length > 1)
        {
            throw new ToManyConstructorsException(type.Type, 1, type.Constructors.Length);
        }

        return type.Constructors.Span[0];
    }
}
