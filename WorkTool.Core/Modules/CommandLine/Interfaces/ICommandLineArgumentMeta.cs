namespace WorkTool.Core.Modules.CommandLine.Interfaces;

public interface ICommandLineArgumentMeta<out TType> : IIdentifier<string>
{
    TType Default { get; }

    TType Parse(string value);
}