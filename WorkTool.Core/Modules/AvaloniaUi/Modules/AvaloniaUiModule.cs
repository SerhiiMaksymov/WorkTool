using WorkTool.Core.Modules.AvaloniaUi.Configurations;

namespace WorkTool.Core.Modules.AvaloniaUi.Modules;

public class AvaloniaUiModule : ModularSystemModule
{
    private static readonly DependencyInjector MainDependencyInjector;

    static AvaloniaUiModule()
    {
        var register = new ReadOnlyDependencyInjectorRegister();
        register.RegisterConfiguration<DependencyInjectorConfiguration>();
        MainDependencyInjector = register.Build();
    }

    public AvaloniaUiModule() : base(MainDependencyInjector) { }
}
