namespace WorkTool.Core.Modules.CommandLine.Models;

public class CommandLineArgumentMetaCollections
    : IEnumerable<KeyValuePair<string, ICommandLineArgumentMeta<object>>>
{
    public static readonly CommandLineArgumentMetaCollections Empty =
        new(Enumerable.Empty<ICommandLineArgumentMeta<object>>());

    private readonly Dictionary<string, ICommandLineArgumentMeta<object>> metas;

    public ICommandLineArgumentMeta<object> this[string key] => metas[key];

    public CommandLineArgumentMetaCollections(IEnumerable<ICommandLineArgumentMeta<object>> metas)
    {
        this.metas = new Dictionary<string, ICommandLineArgumentMeta<object>>();

        foreach (var meta in metas)
        {
            this.metas.Add(meta.Key, meta);
        }
    }

    public IEnumerator<KeyValuePair<string, ICommandLineArgumentMeta<object>>> GetEnumerator()
    {
        return metas.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return metas.GetEnumerator();
    }
}
