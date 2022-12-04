namespace WorkTool.Core.Modules.CommandLine.Models;

public class CommandLineContextItem
{
    public CommandLineArgumentMetaCollections              Arguments { get; }
    public Func<IReadOnlyDictionary<string, object>, Task> Action    { get; }

    public CommandLineContextItem(CommandLineArgumentMetaCollections              arguments,
                                  Func<IReadOnlyDictionary<string, object>, Task> action)
    {
        Arguments = arguments;
        Action    = action;
    }
}