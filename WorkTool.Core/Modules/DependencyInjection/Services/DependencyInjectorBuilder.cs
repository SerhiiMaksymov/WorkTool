namespace WorkTool.Core.Modules.DependencyInjection.Services;

public class DependencyInjectorBuilder : IBuilder<IDependencyInjector>, IDependencyInjectorBuilder
{
    private static readonly IRandom<string> DefaultRandomString;

    private readonly Dictionary<AutoInjectIdentifier, InjectorItem> autoInjects;
    private readonly Dictionary<Type, InjectorItem> injectors;
    private readonly Dictionary<ReserveIdentifier, InjectorItem> reserves;
    private IRandom<string> randomString;

    static DependencyInjectorBuilder()
    {
        DefaultRandomString = new RandomStringGuid("N");
    }

    public DependencyInjectorBuilder()
    {
        injectors = new();
        reserves = new();
        autoInjects = new();
        randomString = DefaultRandomString;
    }

    public void AddConfigurationFromAssemblies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        AddConfigurationFromAssemblies(assemblies);
    }

    public void AddConfigurationFromAssemblies(IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            AddConfigurationFromAssembly(assembly);
        }
    }

    public void AddConfigurationFromAssembly(Assembly assembly)
    {
        var types = assembly.GetTypes();
        var dependencyInjectorConfigurations = types
            .Where(
                x =>
                    x is { IsInterface: false, IsAbstract: false }
                    && typeof(IDependencyInjectorConfiguration).IsAssignableFrom(x)
            )
            .ToArray();

        foreach (var type in dependencyInjectorConfigurations)
        {
            var dependencyInjectorConfiguration = Activator
                .CreateInstance(type)
                .ThrowIfNull()
                .ThrowIfIsNot<IDependencyInjectorConfiguration>();

            AddConfiguration(dependencyInjectorConfiguration);
        }
    }

    public void SetRandomString(IRandom<string> newRandomString)
    {
        randomString = newRandomString;
    }

    public void AddConfiguration(IDependencyInjectorConfiguration configuration)
    {
        configuration.Configure(this);
    }

    public void AddConfiguration<TDependencyInjectorConfiguration>()
        where TDependencyInjectorConfiguration : IDependencyInjectorConfiguration, new()
    {
        new TDependencyInjectorConfiguration().Configure(this);
    }

    public IDependencyInjector Build()
    {
        return new DependencyInjector(injectors, reserves, autoInjects, randomString);
    }

    public void RegisterTransient(Type type, Delegate @delegate)
    {
        injectors[type] = new InjectorItem(InjectorItemType.Transient, @delegate);
    }

    public void RegisterSingleton(Type type, Delegate @delegate)
    {
        injectors[type] = new InjectorItem(InjectorItemType.Singleton, @delegate);
    }

    public void RegisterReserveSingleton(ReserveIdentifier identifier, Delegate @delegate)
    {
        reserves[identifier] = new InjectorItem(InjectorItemType.Singleton, @delegate);
    }

    public void RegisterReserveTransient(ReserveIdentifier identifier, Delegate @delegate)
    {
        reserves[identifier] = new InjectorItem(InjectorItemType.Transient, @delegate);
    }

    public void RegisterTransientAutoInject(AutoInjectIdentifier identifier, Delegate @delegate)
    {
        autoInjects[identifier] = new InjectorItem(InjectorItemType.Transient, @delegate);
    }

    public void RegisterSingletonAutoInject(AutoInjectIdentifier identifier, Delegate @delegate)
    {
        autoInjects[identifier] = new InjectorItem(InjectorItemType.Singleton, @delegate);
    }
}
