namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class DesktopAvaloniaUiApplication : AvaloniaUiApplication
{
    public DesktopAvaloniaUiApplication(AppBuilder builder) : base(builder) { }

    public override void Run(string[] args)
    {
        Builder.StartWithClassicDesktopLifetime(args);
    }

    public override Task RunAsync(string[] args)
    {
        return Task.Run(() => Run(args));
    }
}
