namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public abstract class AvaloniaUiApplication : IApplication
{
    protected AvaloniaUiApplication(AppBuilder builder)
    {
        Builder = builder;
    }

    protected AppBuilder Builder { get; }

    public abstract void Run(string[] args);
    public abstract Task RunAsync(string[] args);
}
