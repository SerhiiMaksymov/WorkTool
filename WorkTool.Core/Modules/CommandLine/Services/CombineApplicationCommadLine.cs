namespace WorkTool.Core.Modules.CommandLine.Services;

public class CombineApplicationCommadLine : IApplicationCommadLine
{
    private readonly List<IApplicationCommadLine> applications;

    public CombineApplicationCommadLine(IEnumerable<IApplicationCommadLine> applications)
    {
        this.applications = new List<IApplicationCommadLine>(applications);
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
