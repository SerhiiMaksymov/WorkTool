namespace WorkTool.Console.Helpers;

public static class DependencyInjectorHelper
{
    public static IDependencyInjector CreateIndexOperation()
    {
        return new DependencyInjectorBuilder()
            .RegisterTransient<IManagedNotificationManager>(() =>
            {
                var currentApplication = Application.Current.ThrowIfNull();
                var applicationLifetime = currentApplication.ApplicationLifetime.ThrowIfNull();
                var classicDesktopStyleApplicationLifetime =
                    applicationLifetime.ThrowIfIsNot<IClassicDesktopStyleApplicationLifetime>();
                var mainWindow = classicDesktopStyleApplicationLifetime.MainWindow.ThrowIfNull();

                return new WindowNotificationManager(mainWindow);
            })
            .RegisterTransient<MainView>()
            .RegisterTransient<IMessageBoxView, AvaloniaMessageBoxView>()
            .RegisterTransient<IHumanizing<Exception, object>, ExceptionHumanizing>()
            .RegisterTransient<IHumanizing<Exception, string>, ToStringHumanizing<Exception>>()
            .RegisterTransient<
                IStreamParser<ICommandLineToken, string>,
                CommandLineArgumentParser
            >()
            .RegisterTransient<CommandLineContextBuilder>()
            .RegisterTransient(
                r =>
                    PropertyInfoItemsControlContextBuilder
                        .CreateDefaultBuilder(r.Resolve<IResolver>(), r.Resolve<UiContext>())
                        .Build()
            )
            .RegisterTransient(r => r)
            .RegisterTransient<Control>(
                r =>
                    new DialogControl()
                        .SetName(AvaloniaMessageBoxView.DialogControlName)
                        .SetContent(r.Resolve<MainView>())
                        .SetDialog(r.Resolve<MessageControl>())
            )
            .RegisterTransient(r => (IInvoker)r)
            .RegisterTransient(() => new Uri("avares://App/Styles"))
            .RegisterTransient(
                r =>
                    r.Resolve<UiContextBuilder>()
                        .AddFromAssembly(typeof(WorkToolCoreMarcType).Assembly)
                        .Build()
            )
            .RegisterTransient<IStyleLoader, StyleLoader>()
            .RegisterTransient<IResourceLoader, ResourceLoader>()
            .RegisterTransient(r =>
            {
                var resourceLoader = r.Resolve<IResourceLoader>();

                return resourceLoader.LoadResources();
            })
            .RegisterTransient<IEnumerable<IStyle>>(r =>
            {
                var styleLoader = r.Resolve<IStyleLoader>();

                var result = new List<IStyle>
                {
                    new FluentTheme(r.Resolve<Uri>()) { Mode = FluentThemeMode.Dark },
                    new StyleInclude(r.Resolve<Uri>())
                    {
                        Source = new Uri("avares://Avalonia.Controls.DataGrid/Themes/Fluent.xaml")
                    }
                };

                var styles = styleLoader.LoadStyles();
                result.AddRange(styles);

                return result;
            })
            .RegisterTransient(r =>
            {
                var window = new Window().SetContent(r.Resolve<Control>());

                window.AttachDevTools();

                return window;
            })
            .RegisterTransient<IApplication, DesktopAvaloniaUiApplication>()
            .RegisterTransient(
                r =>
                    AppBuilder
                        .Configure(() => r.Resolve<AvaloniaUiApp>())
                        .UseReactiveUI()
                        .UsePlatformDetect()
            )
            .RegisterTransient<IApplicationCommandLine, CombineApplicationCommandLine>()
            .RegisterTransient<IEnumerable<IApplicationCommandLine>>(
                r => new IApplicationCommandLine[] { r.Resolve<AvaloniaUiApplicationCommandLine>() }
            )
            .RegisterTransient<AvaloniaUiApplicationCommandLine>()
            .RegisterTransient(
                () => new BlobServiceClient(AzureStorageBlobsConnections.Development)
            )
            .RegisterTransient<IRandom<int>>(() => new RandomInt32(new Interval<int>(-99, 99)))
            .RegisterTransient<IRandomArrayItem<Guid>>(() => new RandomArrayItem<Guid>(false))
            .RegisterTransient<IRandomArrayItem<Guid?>>(() => new RandomArrayItem<Guid?>(true))
            .RegisterTransient<IRandomArrayItem<string>>(() => new RandomArrayItem<string>(false))
            .RegisterTransient<IRandomArrayItem<char>>(() => new RandomArrayItem<char>(false))
            .RegisterTransient<IRandom<int, Interval<int>>, RandomInt32InInterval>()
            .Build();
    }
}
