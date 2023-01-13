namespace WorkTool.Console.Helpers;

public static class DependencyInjectorHelper
{
    public static IDependencyInjector CreateDependencyInjector()
    {
        var dependencyInjectorBuilder = new ReadOnlyDependencyInjectorRegister();
        dependencyInjectorBuilder.RegisterConfigurationFromAssemblies();
        dependencyInjectorBuilder.RegisterTransient<CommandLineContextBuilder>();
        dependencyInjectorBuilder.RegisterTransient<IApplication, DesktopAvaloniaUiApplication>();
        dependencyInjectorBuilder.RegisterTransient<AvaloniaUiApplicationCommandLine>();

        dependencyInjectorBuilder.RegisterTransient(() =>
        {
            var window = new Window();
            window.AttachDevTools();

            return window;
        });

        dependencyInjectorBuilder.RegisterTransient(() =>
        {
            var window = new WindowPopup();
            window.AttachDevTools();

            return window;
        });

        dependencyInjectorBuilder.RegisterTransient<IManagedNotificationManager>(() =>
        {
            var currentApplication = Application.Current.ThrowIfNull();
            var applicationLifetime = currentApplication.ApplicationLifetime.ThrowIfNull();
            var classicDesktopStyleApplicationLifetime =
                applicationLifetime.ThrowIfIsNot<IClassicDesktopStyleApplicationLifetime>();
            var mainWindow = classicDesktopStyleApplicationLifetime.MainWindow.ThrowIfNull();

            return new WindowNotificationManager(mainWindow);
        });

        dependencyInjectorBuilder.RegisterTransient<
            IStreamParser<ICommandLineToken, string>,
            CommandLineArgumentParser
        >();

        dependencyInjectorBuilder.RegisterTransient<PropertyInfoTemplatedControlContext>(
            (IResolver resolver, UiContext uiContext) =>
                PropertyInfoItemsControlContextBuilder
                    .CreateDefaultBuilder(resolver, uiContext)
                    .Build()
        );

        dependencyInjectorBuilder.RegisterTransient<
            IApplicationCommandLine,
            CombineApplicationCommandLine
        >();

        dependencyInjectorBuilder.RegisterTransient<IEnumerable<IApplicationCommandLine>>(
            (AvaloniaUiApplicationCommandLine avaloniaUiApplicationCommandLine) =>
                new IApplicationCommandLine[] { avaloniaUiApplicationCommandLine }
        );

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
