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

    public static void RegisterSingleton<T, TImp>(this IRegisterSingleton registerSingleton)
        where T : TImp
        where TImp : notnull
    {
        registerSingleton.RegisterSingleton<T>((IResolver resolver) => resolver.Resolve<TImp>());
    }
}
