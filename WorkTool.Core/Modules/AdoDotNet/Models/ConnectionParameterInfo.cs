namespace WorkTool.Core.Modules.AdoDotNet.Models;

public abstract record ConnectionParameterInfo
{
    private readonly string[] aliases;

    public string              DefaultAlias { get; }
    public IEnumerable<string> Aliases      => aliases;
    public string              DefaultValue { get; }

    protected ConnectionParameterInfo(string defaultAlias, IEnumerable<string> aliases, string defaultValue)
    {
        this.aliases = aliases.ThrowIfNullOrEmpty(nameof(aliases)).ToArray();
        DefaultValue = defaultValue;
        DefaultAlias = defaultAlias.ThrowIfNullOrWhiteSpace(nameof(defaultAlias));
    }
}