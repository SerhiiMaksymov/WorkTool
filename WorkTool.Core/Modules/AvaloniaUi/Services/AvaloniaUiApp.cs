namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class AvaloniaUiApp : AvaloniaApplication
{
    private readonly IResolver resolver;

    public AvaloniaUiApp(IResolver resolver)
    {
        this.resolver = resolver.ThrowIfNull();
    }

    public override void Initialize()
    {
        var styles = resolver.Resolve<IEnumerable<IStyle>>();
        var resourceProviders = resolver.Resolve<IEnumerable<IResourceProvider>>();
        Styles.AddRange(styles);
        Resources.MergedDictionaries.AddRange(resourceProviders);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            desktopLifetime.MainWindow = resolver.Resolve<Window>();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewLifetime)
        {
            singleViewLifetime.MainView = resolver.Resolve<Control>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
