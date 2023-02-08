namespace WorkTool.Core.Modules.DependencyInjection.Extensions;

public static class RegisterSingletonExtension
{
    public static void RegisterSingleton<T>(
        this IRegisterSingleton registerSingleton,
        Expression expression
    )
    {
        registerSingleton.RegisterSingleton(typeof(T), expression);
    }

    public static void RegisterSingleton<T>(this IRegisterSingleton registerSingleton)
    {
        registerSingleton.RegisterSingleton<T>((T value) => value);
    }

    public static void RegisterSingleton<T>(this IRegisterSingleton registerSingleton, T value)
    {
        registerSingleton.RegisterSingleton(typeof(T), () => value);
    }

    public static void RegisterSingleton<T, TImp>(this IRegisterSingleton registerSingleton)
        where TImp : notnull, T
    {
        registerSingleton.RegisterSingleton<T>((TImp imp) => imp);
    }
}
