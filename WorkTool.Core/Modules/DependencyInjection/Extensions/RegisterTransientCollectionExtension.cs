namespace WorkTool.Core.Modules.DependencyInjection.Extensions;

public static class RegisterTransientCollectionExtension
{
    public static void RegisterTransientItem<T>(
        this IRegisterTransientCollection registerTransient,
        Delegate @delegate
    )
    {
        registerTransient.RegisterTransientItem(typeof(T), @delegate);
    }

    public static void RegisterTransientItem<T>(
        this IRegisterTransientCollection registerTransient,
        Func<T> func
    )
    {
        registerTransient.RegisterTransientItem(typeof(T), func);
    }

    public static void RegisterTransientItem<T>(this IRegisterTransientCollection registerTransient)
        where T : notnull
    {
        registerTransient.RegisterTransientItem<T, T>();
    }

    public static void RegisterTransientItem<T, TImp>(
        this IRegisterTransientCollection registerTransient
    ) where TImp : notnull, T
    {
        registerTransient.RegisterTransientItem<T>((TImp imp) => imp);
    }
}
