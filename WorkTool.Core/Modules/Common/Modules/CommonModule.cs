namespace WorkTool.Core.Modules.Common.Modules;

public class CommonModule : ModularSystemModule
{
    private static readonly DependencyInjector MainDependencyInjector;

    private const string IdString = "a8b52bdd-264c-4f94-a195-32749e60edfa";

    public static readonly Guid IdValue = Guid.Parse(IdString);

    static CommonModule()
    {
        var register = new DependencyInjectorRegister();
        register.RegisterConfiguration<CommonDependencyInjectorConfiguration>();
        MainDependencyInjector = register.Build();
    }

    public CommonModule() : base(IdValue, MainDependencyInjector) { }
}
