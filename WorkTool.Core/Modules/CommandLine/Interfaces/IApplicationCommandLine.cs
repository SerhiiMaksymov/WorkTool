namespace WorkTool.Core.Modules.CommandLine.Interfaces;

public interface IApplicationCommandLine : IApplication
{
    bool Contains(string[] args);
}
