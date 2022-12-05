namespace WorkTool.Core.Modules.CommandLine.Services;

public class CommandLineContext
{
    public const string DefaultRoot = "Root";
    private readonly CommandLineContextOptions options;

    private readonly IStreamParser<ICommandLineToken, string> parser;
    public Tree<string, CommandLineContextItem> Tree { get; }

    public CommandLineContext(
        Tree<string, CommandLineContextItem> tree,
        IStreamParser<ICommandLineToken, string> parser,
        CommandLineContextOptions options
    )
    {
        Tree = tree.ThrowIfNull();
        this.parser = parser.ThrowIfNull();
        this.options = options.ThrowIfNull();
    }

    public Task RunAsync(params string[] args)
    {
        var tokens = GetTokens(args).ToArray();

        var names = tokens.OfType<NameCommandLineToken>().Select(x => x.Name).ToArray();

        var item = Tree[names];
        var parameters =
            new Dictionary<ArgumentNameCommandLineToken, ArgumentValueCommandLineToken>();
        var parameterTokens = tokens[names.Length..];

        for (var index = 0; index < parameterTokens.Length; index += 2)
        {
            var name = parameterTokens[index] as ArgumentNameCommandLineToken;
            var value = parameterTokens[index + 1] as ArgumentValueCommandLineToken;
            parameters[name] = value;
        }

        var values = new Dictionary<string, object>();

        foreach (var parameter in parameters)
        {
            var value = item.Value.Arguments[parameter.Key.Name].Parse(parameter.Value.Value);
            values[parameter.Key.Name] = value;
        }

        foreach (var argument in item.Value.Arguments)
        {
            if (values.ContainsKey(argument.Key))
            {
                continue;
            }

            values[argument.Key] = argument.Value.Default;
        }

        return item.Value.Action.Invoke(values);
    }

    public bool Contains(string[] keys)
    {
        return Tree.Contains(keys);
    }

    public IEnumerable<ICommandLineToken> GetTokens(string[] args)
    {
        return parser.Parse(args);
    }
}
