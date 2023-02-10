namespace WorkTool.Core.Modules.MaterialDesign.Modules;

public class MaterialDesignModule : ModularSystemModule
{
    private static readonly DependencyInjector MainDependencyInjector;

    private const string IdString = "f4025752-c717-4f6b-be10-2cf5e64f561a";

    public static readonly Guid IdValue = Guid.Parse(IdString);

    static MaterialDesignModule()
    {
        var register = new DependencyInjectorRegister();
        register.RegisterConfiguration<MaterialDesignDependencyInjectorConfiguration>();
        MainDependencyInjector = register.Build();
    }

    public MaterialDesignModule() : base(IdValue, MainDependencyInjector) { }
}
