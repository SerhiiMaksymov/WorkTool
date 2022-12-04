namespace WorkTool.Core.Modules.CommandLine.Models;

public class ArgumentValueCommandLineToken : ICommandLineToken
{
    public string Value { get; }

    public ArgumentValueCommandLineToken(string value)
    {
        Value = value;
    }
}