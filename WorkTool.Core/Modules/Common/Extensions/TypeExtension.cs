namespace WorkTool.Core.Modules.Common.Extensions;

public static class TypeExtension
{
    public static object? GetDefaultValue(this Type type)
    {
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }

        return null;
    }

    public static bool IsEnumerable(this Type type)
    {
        if (!type.IsGenericType)
        {
            return false;
        }

        if (type.GenericTypeArguments.Length != 1)
        {
            return false;
        }

        var genericType = type.GetGenericTypeDefinition();

        if (genericType != typeof(IEnumerable<>))
        {
            return false;
        }

        return true;
    }
}
