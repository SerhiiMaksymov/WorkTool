[assembly: SupportedOSPlatform("browser")]

namespace WorkTool.WebAssembly;

internal partial class Program
{
    private static void Main(string[] args)
    {
        try
        {
            var dependencyInjectorBuilder = new ReadOnlyDependencyInjectorRegister();
            dependencyInjectorBuilder.RegisterConfigurationFromAssemblies();
            var setup = dependencyInjectorBuilder.Build();
            var browser = new BrowserAvaloniaUiApplication(setup.Resolve<AppBuilder>(), "out");
            browser.Run(args);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }

        /*
    var setup                  = dependencyInjectorBuilder.Build();
    var applicationCommandLine = setup.Resolve<IApplication>();
    applicationCommandLine.Run(args);*/
    }
}
