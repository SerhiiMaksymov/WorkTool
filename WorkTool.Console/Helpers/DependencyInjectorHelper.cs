using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;

namespace WorkTool.Console.Helpers;

public static class DependencyInjectorHelper
{
    public static IDependencyInjector CreateIndexOperation()
    {
        var dependencyInjectorBuilder = new DependencyInjectorBuilder();
        dependencyInjectorBuilder.AddConfigurationFromAssemblies();

        dependencyInjectorBuilder.RegisterTransient<IFileProvider>(
            () => new PhysicalFileProvider(Directory.GetCurrentDirectory())
        );

        dependencyInjectorBuilder.RegisterTransientAutoInject(
            (JsonConfigurationSource x) => x.Path,
            () => "appsettings.json"
        );

        dependencyInjectorBuilder.RegisterTransientAutoInject(
            (JsonConfigurationSource x) => x.FileProvider
        );

        dependencyInjectorBuilder.RegisterTransient<
            JsonConfigurationSource,
            JsonConfigurationSource
        >();

        dependencyInjectorBuilder.RegisterTransient<IList<IConfigurationProvider>>(
            (JsonConfigurationProvider jsonProvider) =>
                new List<IConfigurationProvider> { jsonProvider }
        );

        dependencyInjectorBuilder.RegisterTransient<IConfiguration, ConfigurationRoot>();
        dependencyInjectorBuilder.RegisterTransient<IDelay, DelayService>();
        dependencyInjectorBuilder.RegisterTransient(() => SmsSenderEndpointsOptions.Default);
        dependencyInjectorBuilder.RegisterTransient(() => SmsSenderOptions.Default);
        dependencyInjectorBuilder.RegisterTransient<IManagedNotificationManager>(() =>
        {
            var currentApplication = Application.Current.ThrowIfNull();
            var applicationLifetime = currentApplication.ApplicationLifetime.ThrowIfNull();
            var classicDesktopStyleApplicationLifetime =
                applicationLifetime.ThrowIfIsNot<IClassicDesktopStyleApplicationLifetime>();
            var mainWindow = classicDesktopStyleApplicationLifetime.MainWindow.ThrowIfNull();

            return new WindowNotificationManager(mainWindow);
        });
        dependencyInjectorBuilder.RegisterTransient<MainView>();
        dependencyInjectorBuilder.RegisterTransient<IMessageBoxView, AvaloniaMessageBoxView>();
        dependencyInjectorBuilder.RegisterTransient<
            IHumanizing<Exception, object>,
            ExceptionHumanizing
        >();
        dependencyInjectorBuilder.RegisterTransient<
            IHumanizing<Exception, string>,
            ToStringHumanizing<Exception>
        >();
        dependencyInjectorBuilder.RegisterTransient<
            IStreamParser<ICommandLineToken, string>,
            CommandLineArgumentParser
        >();
        dependencyInjectorBuilder.RegisterTransient<CommandLineContextBuilder>();
        dependencyInjectorBuilder.RegisterTransient<PropertyInfoTemplatedControlContext>(
            (IResolver resolver, UiContext uiContext) =>
                PropertyInfoItemsControlContextBuilder
                    .CreateDefaultBuilder(resolver, uiContext)
                    .Build()
        );

        dependencyInjectorBuilder.RegisterTransient<Control>(
            (MainView mainView, MessageControl messageControl) =>
                new DialogControl()
                    .SetName(AvaloniaMessageBoxView.DialogControlName)
                    .SetContent(mainView)
                    .SetDialog(messageControl)
        );

        dependencyInjectorBuilder.RegisterTransient(() => AppBaseUri.AppStyleUri);
        dependencyInjectorBuilder.RegisterTransient<UiContext>(
            (UiContextBuilder uiContextBuilder) =>
                uiContextBuilder.AddFromAssembly(typeof(WorkToolCoreMarcType).Assembly).Build()
        );

        dependencyInjectorBuilder.RegisterTransient<IStyleLoader, StyleLoader>();
        dependencyInjectorBuilder.RegisterTransient<IResourceLoader, ResourceLoader>();

        dependencyInjectorBuilder.RegisterTransient<IEnumerable<IResourceProvider>>(
            (IResourceLoader resourceLoader) => resourceLoader.LoadResources()
        );

        dependencyInjectorBuilder.RegisterTransient<IEnumerable<IStyle>>(
            (IStyleLoader styleLoader, Uri uri) =>
            {
                var result = new List<IStyle>
                {
                    new FluentTheme(uri) { Mode = FluentThemeMode.Dark },
                    new StyleInclude(uri) { Source = AvaloniaUriBase.DataGridThemeFluentUri },
                    new StyleInclude(uri) { Source = AvaloniaUriBase.MaterialIconsUri }
                };

                var styles = styleLoader.LoadStyles();
                result.AddRange(styles);

                result.Add(new StyleInclude(uri) { Source = AvaloniaUriBase.ControlsStylesUri });

                return result;
            }
        );

        dependencyInjectorBuilder.RegisterTransient<Window>(
            (Control control) =>
            {
                var window = new Window().SetContent(control);

                window.AttachDevTools();

                return window;
            }
        );
        dependencyInjectorBuilder.RegisterTransient<IApplication, DesktopAvaloniaUiApplication>();
        dependencyInjectorBuilder.RegisterTransient<AppBuilder>(
            (AvaloniaUiApp avaloniaUiApp) =>
                AppBuilder.Configure(() => avaloniaUiApp).UseReactiveUI().UsePlatformDetect()
        );
        dependencyInjectorBuilder.RegisterTransient<
            IApplicationCommandLine,
            CombineApplicationCommandLine
        >();
        dependencyInjectorBuilder.RegisterTransient<IEnumerable<IApplicationCommandLine>>(
            (AvaloniaUiApplicationCommandLine avaloniaUiApplicationCommandLine) =>
                new IApplicationCommandLine[] { avaloniaUiApplicationCommandLine }
        );
        dependencyInjectorBuilder.RegisterTransient<AvaloniaUiApplicationCommandLine>();
        dependencyInjectorBuilder.RegisterTransient(
            () => new BlobServiceClient(AzureStorageBlobsConnections.Development)
        );

        dependencyInjectorBuilder.RegisterTransient<IRandom<int>>(
            () => new RandomInt32(new Interval<int>(-99, 99))
        );
        dependencyInjectorBuilder.RegisterTransient<IRandomArrayItem<Guid>>(
            () => new RandomArrayItem<Guid>(false)
        );
        dependencyInjectorBuilder.RegisterTransient<IRandomArrayItem<Guid?>>(
            () => new RandomArrayItem<Guid?>(true)
        );

        dependencyInjectorBuilder.RegisterTransient<IRandomArrayItem<string>>(
            () => new RandomArrayItem<string>(false)
        );
        dependencyInjectorBuilder.RegisterTransient<IRandomArrayItem<char>>(
            () => new RandomArrayItem<char>(false)
        );
        dependencyInjectorBuilder.RegisterTransient<
            IRandom<int, Interval<int>>,
            RandomInt32InInterval
        >();
        return dependencyInjectorBuilder.Build();
    }
}
