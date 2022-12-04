namespace WorkTool.Core.Modules.CommandLine.Models;

public abstract class CommandLineArgumentMeta<TType> : Identifier<string>, ICommandLineArgumentMeta<TType>
{
    protected CommandLineArgumentMeta(string key, TType @default) : base(key)
    {
        Default = @default;
    }

    public TType Default { get; }

    public abstract TType Parse(string value);
}