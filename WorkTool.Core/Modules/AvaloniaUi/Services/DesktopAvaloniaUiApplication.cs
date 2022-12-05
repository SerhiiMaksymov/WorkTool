namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class DesktopAvaloniaUiApplication : IApplication
{
    public AppBuilder Builder { get; }

    public DesktopAvaloniaUiApplication(AppBuilder builder)
    {
        Builder = builder.ThrowIfNull();
    }

    public Task RunAsync(string[] args)
    {
        return Task.Run(() => Builder.StartWithClassicDesktopLifetime(args));
    }
}
