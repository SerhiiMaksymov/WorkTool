namespace WorkTool.Core.Modules.Ui.Models;

public class FunctionsInfo
{
    public string Name { get; }
    public Delegate Command { get; }

    public FunctionsInfo(string name, Delegate command)
    {
        Name = name.ThrowIfNullOrWhiteSpace();
        Command = command.ThrowIfNull();
    }
}
