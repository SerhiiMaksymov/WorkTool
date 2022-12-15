namespace WorkTool.Core.Modules.DependencyInjection.Extensions;

public static class RegisterTransientExtension
{
    public static void RegisterTransient<T>(
        this IRegisterTransient registerTransient,
        Delegate @delegate
    )
    {
        registerTransient.RegisterTransient(typeof(T), @delegate);
    }

    public static void RegisterTransient<T>(this IRegisterTransient registerTransient, Func<T> func)
    {
        registerTransient.RegisterTransient(typeof(T), func);
    }

    public static void RegisterTransient<T>(this IRegisterTransient registerTransient)
        where T : notnull
    {
        registerTransient.RegisterTransient<T, T>();
    }

    public static void RegisterTransient<T, TImp>(this IRegisterTransient registerTransient)
        where TImp : notnull, T
    {
        registerTransient.RegisterTransient<T>((TImp imp) => imp);
    }
}
