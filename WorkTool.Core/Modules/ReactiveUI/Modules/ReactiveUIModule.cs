namespace WorkTool.Core.Modules.ReactiveUI.Modules;

public class ReactiveUIModule : ModularSystemModule
{
    private static readonly DependencyInjector MainDependencyInjector;

    private const string IdString = "f59c0a7c-39d8-4584-b623-64f99a7fb8ee";

    public static readonly Guid IdValue = Guid.Parse(IdString);

    static ReactiveUIModule()
    {
        var register = new ReadOnlyDependencyInjectorRegister();
        register.RegisterConfiguration<ReactiveUIDependencyInjectorConfiguration>();
        MainDependencyInjector = register.Build();
    }

    public ReactiveUIModule() : base(IdValue, MainDependencyInjector) { }
}
