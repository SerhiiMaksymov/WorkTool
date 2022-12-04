namespace WorkTool.Core.Modules.CommandLine.Models;

public class NameCommandLineToken : ICommandLineToken
{
    public string Name { get; set; }

    public NameCommandLineToken(string name)
    {
        Name = name;
    }
}