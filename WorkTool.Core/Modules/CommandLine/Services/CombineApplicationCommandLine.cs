namespace WorkTool.Core.Modules.CommandLine.Services;

public class CombineApplicationCommandLine : IApplicationCommandLine
{
    private readonly List<IApplicationCommandLine> applications;

    public CombineApplicationCommandLine(IEnumerable<IApplicationCommandLine> applications)
    {
        this.applications = new List<IApplicationCommandLine>(applications);
    }

    public bool Contains(string[] args)
    {
        foreach (var application in applications)
        {
            if (application.Contains(args))
            {
                return true;
            }
        }

        return false;
    }

    public Task RunAsync(string[] args)
    {
        foreach (var application in applications)
        {
            if (!application.Contains(args))
            {
                continue;
            }

            return application.RunAsync(args);
        }

        throw new Exception($"Not know {args}.");
    }
}
