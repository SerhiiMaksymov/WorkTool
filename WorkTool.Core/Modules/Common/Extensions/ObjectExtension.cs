﻿namespace WorkTool.Core.Modules.Common.Extensions;

public static class ObjectExtension
{
    public static T ThrowIfIsNot<T>(this  object obj)
    {
        if (obj is not T result)
        {
            throw new TypeInvalidCastException(typeof(T), obj.GetType());
        }

        return result;
    }

    public static T With<T>(this T value, Action<T> action)
    {
        action.Invoke(value);

        return value;
    }

    public static T? As<T>(this object value) where T : class
    {
        return value as T;
    }

    public static T ThrowIfNull<T>(
        this T? obj,
        [CallerArgumentExpression(nameof(obj))] string paramName = ""
    )
    {
        return obj ?? throw new ArgumentNullException(paramName);
    }

    public static TObj ThrowIfNotEquals<TObj>(this TObj obj, TObj expected, string name)
        where TObj : notnull
    {
        if (obj.Equals(expected))
        {
            return obj;
        }

        throw new NotEqualsExtension<TObj>(name, obj, expected);
    }
}
