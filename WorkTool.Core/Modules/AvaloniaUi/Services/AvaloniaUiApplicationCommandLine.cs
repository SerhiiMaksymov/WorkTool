namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class AvaloniaUiApplicationCommandLine : IApplicationCommandLine
{
    public const string CommandName = "AvaloniaUi";
    public static readonly CommandLineContextItem Default =
        new(
            CommandLineArgumentMetaCollections.Empty,
            _ => throw new Exception("Default CommandLineContextItem.")
        );
    private readonly CommandLineContext commandLineContext;
    private readonly IResolver resolver;

    public AvaloniaUiApplicationCommandLine(IResolver resolver)
    {
        this.resolver = resolver;
        var parser = new CommandLineArgumentParser();
        var builder = new CommandLineContextBuilder(parser);
        AddCommand(builder);
        commandLineContext = builder.Build();
    }

    public bool Contains(string[] args)
    {
        var names = commandLineContext
            .GetTokens(args)
            .OfType<NameCommandLineToken>()
            .Select(x => x.Name)
            .ToArray();

        return commandLineContext.Contains(names);
    }

    public Task RunAsync(string[] args)
    {
        return commandLineContext.RunAsync(args);
    }

    private void AddCommand(CommandLineContextBuilder builder)
    {
        builder[Default, CommandLineContext.DefaultRoot, CommandName] = new TreeNodeBuilder<
            string,
            CommandLineContextItem?
        >
        {
            Value = new CommandLineContextItem(
                new CommandLineArgumentMetaCollections(new ICommandLineArgumentMeta<object>[] { }),
                _ =>
                {
                    var desktopAvaloniaUiApplication =
                        resolver.Resolve<DesktopAvaloniaUiApplication>();

                    return desktopAvaloniaUiApplication.RunAsync(new string[0]);
                }
            )
        };
    }
}
