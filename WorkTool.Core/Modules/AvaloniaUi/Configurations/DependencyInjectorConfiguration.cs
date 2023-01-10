namespace WorkTool.Core.Modules.AvaloniaUi.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister dependencyInjectorRegister)
    {
        dependencyInjectorRegister.RegisterTransient(() => UriBase.AppStyleUri);
        dependencyInjectorRegister.RegisterTransientAutoInject((AvaloniaUiApp app) => app.Resolver);
        dependencyInjectorRegister.RegisterTransient<IStyleLoader, StyleLoader>();
        dependencyInjectorRegister.RegisterTransient<IResourceLoader, ResourceLoader>();
        dependencyInjectorRegister.RegisterTransient(() => new Window());
        ConfigureViewModels(dependencyInjectorRegister);
        dependencyInjectorRegister.RegisterTransient(() => Enumerable.Empty<IStyle>());

        dependencyInjectorRegister.RegisterTransient<Control>(
            (MainView mainView, MessageControl messageControl) =>
                new DialogControl()
                    .SetName(DialogControlMessageBoxView.DialogControlName)
                    .SetContent(mainView)
                    .SetDialog(messageControl)
        );

        dependencyInjectorRegister.RegisterTransient<
            IMessageBoxView,
            DialogControlMessageBoxView
        >();

        dependencyInjectorRegister.RegisterTransientAutoInject(
            (Window window) => window.Content,
            (Control control) => control
        );

        dependencyInjectorRegister.RegisterTransient<IEnumerable<IResourceProvider>>(
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
