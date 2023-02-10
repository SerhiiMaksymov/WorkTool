using WorkTool.Core.Modules.AutoMapper.Configurations;

namespace WorkTool.Core.Modules.AutoMapper.Modules;

public class AutoMapperModule : ModularSystemModule
{
    private static readonly DependencyInjector MainDependencyInjector;

    private const string IdString = "e7b1fb1d-dc39-4c82-ab21-a14526f0d0e3";

    public static readonly Guid IdValue = Guid.Parse(IdString);

    static AutoMapperModule()
    {
        var register = new DependencyInjectorRegister();
        register.RegisterConfiguration<AutoMapperDependencyInjectorConfiguration>();
        MainDependencyInjector = register.Build();
    }

    public AutoMapperModule() : base(IdValue, MainDependencyInjector) { }
}
