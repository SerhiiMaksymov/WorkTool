namespace WorkTool.Core.Modules.DependencyInjection.Services;

public class ReadOnlyDependencyInjectorRegister
    : IBuilder<ReadOnlyReadOnlyDependencyInjector>,
        IDependencyInjectorRegister
{
    private static readonly IRandom<string> DefaultRandomString;

    private readonly Dictionary<AutoInjectIdentifier, InjectorItem> autoInjects;
    private readonly Dictionary<Type, InjectorItem> injectors;
    private readonly Dictionary<ReserveIdentifier, InjectorItem> reserves;
    private IRandom<string> randomString;

    static ReadOnlyDependencyInjectorRegister()
    {
        DefaultRandomString = RandomStringGuid.Digits;
    }

    public ReadOnlyDependencyInjectorRegister()
    {
        injectors = new();
        reserves = new();
        autoInjects = new();
        randomString = DefaultRandomString;
    }

    public void RegisterConfiguration(IDependencyInjectorConfiguration configuration)
    {
        configuration.Configure(this);
    }

    public void SetRandomString(IRandom<string> newRandomString)
    {
        randomString = newRandomString;
    }

    public ReadOnlyReadOnlyDependencyInjector Build()
    {
        return new ReadOnlyReadOnlyDependencyInjector(
            injectors,
            reserves,
            autoInjects,
            randomString
        );
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
