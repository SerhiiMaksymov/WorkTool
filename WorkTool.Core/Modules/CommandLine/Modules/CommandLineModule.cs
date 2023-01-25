namespace WorkTool.Core.Modules.CommandLine.Modules;

public class CommandLineModule : ModularSystemModule
{
    private static readonly DependencyInjector MainDependencyInjector;

    private const string IdString = "3a4c65c2-9286-44fe-a1d7-bae64ca9ab1e";

    public static readonly Guid IdValue = Guid.Parse(IdString);

    static CommandLineModule()
    {
        var register = new ReadOnlyDependencyInjectorRegister();
        register.RegisterConfiguration<CommandLineDependencyInjectorConfiguration>();
        MainDependencyInjector = register.Build();
    }

    public CommandLineModule() : base(IdValue, MainDependencyInjector) { }
}
