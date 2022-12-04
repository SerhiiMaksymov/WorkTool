var setup                  = DependencyInjectorHelper.CreateIndexOperation();
var applicationCommandLine = setup.Resolve<IApplicationCommadLine>();

var arguments = new List<string>
{
    "Root"
};

arguments.AddRange(args);
var argsArray = arguments.ToArray();
await applicationCommandLine.RunAsync(argsArray);