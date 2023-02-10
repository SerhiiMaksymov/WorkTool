namespace WorkTool.Core.Modules.Configuration.Modules;

public class ConfigurationModule : ModularSystemModule
{
    private static readonly DependencyInjector MainDependencyInjector;

    private const string IdString = "769d3a4f-5795-4bc3-b29c-213d1c4516a2";

    public static readonly Guid IdValue = Guid.Parse(IdString);

    static ConfigurationModule()
    {
        var register = new DependencyInjectorRegister();
        register.RegisterConfiguration<ConfigurationDependencyInjectorConfiguration>();
        MainDependencyInjector = register.Build();
    }

    public ConfigurationModule() : base(IdValue, MainDependencyInjector) { }
}
