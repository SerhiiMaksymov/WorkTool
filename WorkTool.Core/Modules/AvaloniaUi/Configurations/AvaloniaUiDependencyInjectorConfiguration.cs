namespace WorkTool.Core.Modules.AvaloniaUi.Configurations;

public readonly struct AvaloniaUiDependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterTransient<AvaloniaUiApp>();
        register.RegisterTransient(() => UriBase.AppStyleUri);
        register.RegisterTransient<DialogControlMessageBoxView>();
        register.RegisterTransient<AvaloniaUiApplicationCommandLine>();
        register.RegisterTransient<IResourceLoader, ResourceLoader>();
        register.RegisterTransient<MessageControl>();
        register.RegisterTransient<MainView>();
        register.RegisterTransient<ViewModelBase>();
        register.RegisterTransient<UiContextBuilder>();
        register.RegisterTransient<AppViewLocatorBuilder>();
        register.RegisterTransient<FluentTheme>((Uri uri) => new FluentTheme(uri));
        register.RegisterTransient(() => new Window());
        register.RegisterTransient<IMessageBoxView, DialogControlMessageBoxView>();
        ConfigureViewModels(register);

        register.RegisterTransientAutoInject(
            (AvaloniaUiApp avaloniaUiApp) => avaloniaUiApp.Resolver
        );

        register.RegisterTransient<IEnumerable<IStyle>>(
            (FluentTheme fluentTheme, Uri uri) =>
                new IStyle[]
                {
                    fluentTheme,
                    new StyleInclude(uri) { Source = UriBase.ControlsStylesUri }
                }
        );

        register.RegisterTransient<IViewLocator>(
            (AppViewLocatorBuilder builder) => builder.Build()
        );

        register.RegisterTransient<Control>(
            (MessageControl messageControl, RoutedViewHost routedViewHost) =>
                new DialogControl()
                    .SetName(DialogControlMessageBoxView.DialogControlName)
                    .SetContent(routedViewHost)
                    .SetDialog(messageControl)
        );

        register.RegisterTransient<RoutedViewHost>(
            (MainView mainView, IViewLocator viewLocator) =>
                new RoutedViewHost().SetDefaultContent(mainView).SetViewLocator(viewLocator)
        );

        register.RegisterTransientAutoInject(
            (Window window) => window.Content,
            (IControl control) => control
        );

        register.RegisterTransient<IEnumerable<IResourceProvider>>(
            (IResourceLoader resourceLoader) => resourceLoader.LoadResources()
        );
    }

    private void ConfigureViewModels(IDependencyInjectorRegister register)
    {
        var styledElementType = typeof(StyledElement);
        var member = styledElementType.GetMember(nameof(StyledElement.DataContext))[0];
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var autoInjectMember = new AutoInjectMember(member);

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                if (type.Namespace.IsNullOrWhiteSpace())
                {
                    continue;
                }

                if (!styledElementType.IsAssignableFrom(type))
                {
                    continue;
                }

                var ns = type.Namespace
                    .Replace(".Views.", ".ViewModels.")
                    .Replace(".Views", ".ViewModels");

                var viewModelName = $"{ns}.{type.Name}Model";
                var viewModelType = assembly.GetType(viewModelName);

                if (viewModelType is null)
                {
                    continue;
                }

                var autoInjectIdentifier = new AutoInjectMemberIdentifier(type, autoInjectMember);
                var variable = viewModelType.ToVariableAutoName();
                register.RegisterTransient(type);
                register.RegisterTransient(viewModelType);

                register.RegisterTransientAutoInjectMember(
                    autoInjectIdentifier,
                    variable.ToLambda(variable)
                );
            }
        }
    }
}
