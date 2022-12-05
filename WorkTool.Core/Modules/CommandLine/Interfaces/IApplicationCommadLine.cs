namespace WorkTool.Core.Modules.CommandLine.Interfaces;

public interface IApplicationCommadLine : IApplication
{
    bool Contains(string[] args);
}
