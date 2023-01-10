namespace WorkTool.Console;

class Program
{
    public static async Task Main(string[] args)
    {
        var setup = DependencyInjectorHelper.CreateDependencyInjector();
        var applicationCommandLine = setup.Resolve<IApplicationCommandLine>();
        var arguments = new List<string> { "Root" };
        arguments.AddRange(args);
        var argsArray = arguments.ToArray();
        await applicationCommandLine.RunAsync(argsArray);
    }
}
