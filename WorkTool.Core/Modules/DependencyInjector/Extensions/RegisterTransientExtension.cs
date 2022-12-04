namespace WorkTool.Core.Modules.DependencyInjector.Extensions;

public static class RegisterTransientExtension
{
    public static TRegisterTransient RegisterTransient<TRegisterTransient, TObject>(
    this TRegisterTransient registerTransient,
    Func<IResolver, object> func) where TRegisterTransient : IRegisterTransient
    {
        registerTransient.RegisterTransient(typeof(TObject), func);

        return registerTransient;
    }
}