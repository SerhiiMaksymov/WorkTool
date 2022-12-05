namespace WorkTool.Core.Modules.CommandLine.Services;

public sealed class CommandLineContextBuilder : IBuilder<CommandLineContext>
{
    private readonly CommandLineContextOptions options;
    private readonly IStreamParser<ICommandLineToken, string> streamParser;
    private readonly TreeBuilder<string, CommandLineContextItem> treeBuilder;

    public TreeNodeBuilder<string, CommandLineContextItem> this[string key]
    {
        get => treeBuilder[key];
        set => treeBuilder[key] = value;
    }

    public TreeNodeBuilder<
        string,
        CommandLineContextItem
    > this[string key, CommandLineContextItem defaultValue] => treeBuilder[key, defaultValue];

    public TreeNodeBuilder<
        string,
        CommandLineContextItem
    > this[CommandLineContextItem defaultValue, params string[] keys]
    {
        get => treeBuilder[defaultValue, keys];
        set => treeBuilder[defaultValue, keys] = value;
    }

    public TreeNodeBuilder<string, CommandLineContextItem> this[params string[] keys]
    {
        get => treeBuilder[keys];
        set => treeBuilder[keys] = value;
    }

    public CommandLineContextBuilder(IStreamParser<ICommandLineToken, string> streamParser)
    {
        this.streamParser = streamParser.ThrowIfNull();
        options = new CommandLineContextOptions();
        treeBuilder = new TreeBuilder<string, CommandLineContextItem>();
    }

    public CommandLineContext Build()
    {
        var tree = treeBuilder.Build();

        return new CommandLineContext(
            tree,
            streamParser,
            new CommandLineContextOptions { RootKey = treeBuilder.Root.Key }
        );
    }
}
