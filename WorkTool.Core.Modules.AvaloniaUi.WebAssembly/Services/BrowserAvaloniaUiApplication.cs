namespace WorkTool.Core.Modules.AvaloniaUi.WebAssembly.Services;

public class BrowserAvaloniaUiApplication : AvaloniaUiApplication
{
    private readonly string mainDivId;

    public BrowserAvaloniaUiApplication(AppBuilder builder, string mainDivId) : base(builder)
    {
        this.mainDivId = mainDivId.ThrowIfNullOrWhiteSpace();
    }

    public override void Run(string[] args)
    {
        Builder.SetupBrowserApp(mainDivId);
    }

    public override Task RunAsync(string[] args)
    {
        return Task.Run(() => Run(args));
    }
}
