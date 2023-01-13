namespace WorkTool.Core.Modules.AvaloniaUi.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterTransient(() => UriBase.AppStyleUri);
        register.RegisterTransientAutoInject((AvaloniaUiApp app) => app.Resolver);
        register.RegisterTransient<IStyleLoader, StyleLoader>();
        register.RegisterTransient<IResourceLoader, ResourceLoader>();
        register.RegisterTransientItem<IStyle>((Uri uri) => new FluentTheme(uri));
        register.RegisterTransient(() => new Window());
        ConfigureViewModels(register);

        register.RegisterTransientItem<IStyle>(
            (Uri uri) => new StyleInclude(uri) { Source = UriBase.ControlsStylesUri }
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

        register.RegisterTransient<IMessageBoxView, DialogControlMessageBoxView>();

        register.RegisterTransientAutoInject(
            (Window window) => window.Content,
            (Control control) => control
        );

        register.RegisterTransient<IEnumerable<IResourceProvider>>(
            (IResourceLoader resourceLoader) => resourceLoader.LoadResources()
        );
    }

    private void ConfigureViewModels(IDependencyInjectorRegister dependencyInjectorRegister)
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

                var autoInjectIdentifier = new AutoInjectIdentifier(type, autoInjectMember);

                dependencyInjectorRegister.RegisterTransientAutoInject(
                    autoInjectIdentifier,
                    (IResolver resolver) => resolver.Resolve(viewModelType)
                );
            }
        }
    }
}
