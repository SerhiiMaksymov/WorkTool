namespace WorkTool.Core.Modules.CommandLine.Interfaces;

public interface ICommandLineArgument<out TValue>
{
    ICommandLineArgumentMeta<TValue> Meta  { get; }
    TValue                           Value { get; }
}