namespace WorkTool.Core.Modules.DependencyInjection.Extensions;

public static class RegisterSingletonExtension
{
    public static void RegisterSingleton<T>(
        this IRegisterSingleton registerSingleton,
        Delegate @delegate
    )
    {
        registerSingleton.RegisterSingleton(typeof(T), @delegate);
    }

    public static void RegisterSingleton<T>(this IRegisterSingleton registerSingleton, object value)
    {
        registerSingleton.RegisterSingleton(typeof(T), () => value);
    }

    public static void RegisterSingleton<T, TImp>(this IRegisterSingleton registerSingleton)
        where TImp : notnull, T
    {
        registerSingleton.RegisterSingleton<T>((TImp imp) => imp);
    }
}
