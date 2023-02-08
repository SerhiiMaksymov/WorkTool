namespace WorkTool.Core.Modules.AvaloniaUi.Modules;

public class AvaloniaUiDesktopModule : ModularSystemModule
{
    private static readonly DependencyInjector MainDependencyInjector;

    private const string IdString = "b05188e3-4519-43f8-ba9d-2372e69ce331";

    public static readonly Guid IdValue = Guid.Parse(IdString);

    static AvaloniaUiDesktopModule()
    {
        var register = new ReadOnlyDependencyInjectorRegister();
        register.RegisterConfiguration<AvaloniaUiDesktopDependencyInjectorConfiguration>();
        MainDependencyInjector = register.Build();
    }

    public AvaloniaUiDesktopModule() : base(IdValue, MainDependencyInjector) { }
}
