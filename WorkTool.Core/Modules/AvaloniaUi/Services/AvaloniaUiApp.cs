namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class AvaloniaUiApp : AvaloniaApplication
{
    public IResolver? Resolver { get; set; }

    public override void Initialize()
    {
        Resolver = Resolver.ThrowIfNull();
        var styles = Resolver.Resolve<IEnumerable<IStyle>>();
        var resourceProviders = Resolver.Resolve<IEnumerable<IResourceProvider>>();
        Styles.AddRange(styles);
        Resources.MergedDictionaries.AddRange(resourceProviders);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();
        Resolver = Resolver.ThrowIfNull();

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktopLifetime:
            {
                desktopLifetime.MainWindow = Resolver.Resolve<Window>();

                break;
            }
            case ISingleViewApplicationLifetime singleViewLifetime:
            {
                singleViewLifetime.MainView = Resolver.Resolve<Control>();

                break;
            }
            default:
            {
                throw new UnreachableException();
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}
