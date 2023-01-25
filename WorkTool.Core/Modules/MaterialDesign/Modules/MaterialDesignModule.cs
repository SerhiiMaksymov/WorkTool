using WorkTool.Core.Modules.MaterialDesign.Configurations;

namespace WorkTool.Core.Modules.MaterialDesign.Modules;

public class MaterialDesignModule : ModularSystemModule
{
    private static readonly DependencyInjector MainDependencyInjector;

    private const string IdString = "d42f537a-1271-45b6-8658-0bd4792e6e5c";

    public static readonly Guid IdValue = Guid.Parse(IdString);

    static MaterialDesignModule()
    {
        var register = new ReadOnlyDependencyInjectorRegister();
        register.RegisterConfiguration<MaterialDesignDependencyInjectorConfiguration>();
        MainDependencyInjector = register.Build();
    }

    public MaterialDesignModule() : base(IdValue, MainDependencyInjector) { }
}
