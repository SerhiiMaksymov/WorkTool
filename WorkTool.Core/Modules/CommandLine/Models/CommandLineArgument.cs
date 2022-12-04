namespace WorkTool.Core.Modules.CommandLine.Models;

public class CommandLineArgument<TValue> : ICommandLineArgument<TValue>
{
    public CommandLineArgument(ICommandLineArgumentMeta<TValue> meta, TValue value)
    {
        Meta  = meta.ThrowIfNull(nameof(meta));
        Value = value.ThrowIfNull(nameof(value));
    }

    public ICommandLineArgumentMeta<TValue> Meta  { get; }
    public TValue                           Value { get; }
}