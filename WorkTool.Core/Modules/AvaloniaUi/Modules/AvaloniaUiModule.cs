namespace WorkTool.Core.Modules.AvaloniaUi.Modules;

public class AvaloniaUiModule : ModularSystemModule
{
    private static readonly DependencyInjector MainDependencyInjector;

    private const string IdString = "09d83989-8e6f-40e7-ab60-1fff19b9e44d";

    public static readonly Guid IdValue = Guid.Parse(IdString);

    static AvaloniaUiModule()
    {
        var register = new ReadOnlyDependencyInjectorRegister();
        register.RegisterConfiguration<AvaloniaUiDependencyInjectorConfiguration>();
        MainDependencyInjector = register.Build();
    }

    public AvaloniaUiModule() : base(IdValue, MainDependencyInjector) { }
}
