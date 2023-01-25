namespace WorkTool.Console.Helpers;

public static class DependencyInjectorHelper
{
    public static IDependencyInjector CreateDependencyInjector()
    {
        var register = new ReadOnlyDependencyInjectorRegister();
        register.RegisterTransient<IApplicationCommandLine, CombineApplicationCommandLine>();
        register.RegisterConfigurationFromAssemblies();
        register.RegisterTransient<CommandLineContextBuilder>();
        register.RegisterTransient<IApplication, DesktopAvaloniaUiApplication>();
        register.RegisterTransient<AvaloniaUiApplicationCommandLine>();

        register.RegisterTransient(() =>
        {
            var window = new Window();
            window.AttachDevTools();

            return window;
        });

        register.RegisterTransient(() =>
        {
            var window = new WindowPopup();
            window.AttachDevTools();

            return window;
        });

        register.RegisterTransient<IManagedNotificationManager>(() =>
        {
            var currentApplication = Application.Current.ThrowIfNull();
            var applicationLifetime = currentApplication.ApplicationLifetime.ThrowIfNull();
            var classicDesktopStyleApplicationLifetime =
                applicationLifetime.ThrowIfIsNot<IClassicDesktopStyleApplicationLifetime>();
            var mainWindow = classicDesktopStyleApplicationLifetime.MainWindow.ThrowIfNull();

            return new WindowNotificationManager(mainWindow);
        });

        register.RegisterTransient<
            IStreamParser<ICommandLineToken, string>,
            CommandLineArgumentParser
        >();

        register.RegisterTransient<PropertyInfoTemplatedControlContext>(
            (IResolver resolver, UiContext uiContext) =>
                PropertyInfoItemsControlContextBuilder
                    .CreateDefaultBuilder(resolver, uiContext)
                    .Build()
        );

        register.RegisterTransient<IEnumerable<IApplicationCommandLine>>(
            (AvaloniaUiApplicationCommandLine avaloniaUiApplicationCommandLine) =>
                new IApplicationCommandLine[] { avaloniaUiApplicationCommandLine }
        );

        register.RegisterTransient(
            () => new BlobServiceClient(AzureStorageBlobsConnections.Development)
        );

        register.RegisterTransient<IRandom<int>>(() => new RandomInt32(new Interval<int>(-99, 99)));

        register.RegisterTransient<IRandomArrayItem<Guid>>(() => new RandomArrayItem<Guid>(false));

        register.RegisterTransient<IRandomArrayItem<Guid?>>(() => new RandomArrayItem<Guid?>(true));

        register.RegisterTransient<IRandomArrayItem<string>>(
            () => new RandomArrayItem<string>(false)
        );

        register.RegisterTransient<IRandomArrayItem<char>>(() => new RandomArrayItem<char>(false));

        register.RegisterTransient<IRandom<int, Interval<int>>, RandomInt32InInterval>();

        return register.Build();
    }
}
