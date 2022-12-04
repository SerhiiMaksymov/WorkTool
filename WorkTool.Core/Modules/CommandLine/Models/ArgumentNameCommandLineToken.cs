namespace WorkTool.Core.Modules.CommandLine.Models;

public class ArgumentNameCommandLineToken : ICommandLineToken
{
    public string Name { get; set; }

    public ArgumentNameCommandLineToken(string name)
    {
        Name = name;
    }
}