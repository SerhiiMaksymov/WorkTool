namespace WorkTool.Core.Modules.Common.Extensions;

public static class ObjectExtension
{
    public static T With<T>(this T value, Action<T> action)
    {
        action.Invoke(value);

        return value;
    }

    public static T As<T>(this object value) where T : class
    {
        return value as T;
    }

    public static T ThrowIfNull<T>(
        this T obj,
        [CallerArgumentExpression("obj")] string paramName = ""
    )
    {
        return obj is null ? throw new ArgumentNullException(paramName) : obj;
    }

    public static TObj ThrowIfNotEquals<TObj>(this TObj obj, TObj expected, string name)
    {
        if (obj.Equals(expected))
        {
            return obj;
        }

        throw new NotEqualsExtension<TObj>(name, obj, expected);
    }
}
