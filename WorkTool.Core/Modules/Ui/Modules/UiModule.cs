namespace WorkTool.Core.Modules.Ui.Modules;

public class UiModule : ModularSystemModule
{
    private static readonly DependencyInjector MainDependencyInjector;

    private const string IdString = "e5f3149e-adb0-41a1-ac92-40734d793781";

    public static readonly Guid IdValue = Guid.Parse(IdString);

    static UiModule()
    {
        var register = new ReadOnlyDependencyInjectorRegister();
        register.RegisterConfiguration<UiDependencyInjectorConfiguration>();
        MainDependencyInjector = register.Build();
    }

    public UiModule() : base(IdValue, MainDependencyInjector) { }
}
