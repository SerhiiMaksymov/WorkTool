namespace WorkTool.Core.Modules.DependencyInjection.Services;

public class ReadOnlyDependencyInjectorRegister
    : IBuilder<DependencyInjector>,
        IDependencyInjectorRegister
{
    private static readonly IRandom<string> DefaultRandomString;

    private readonly Dictionary<AutoInjectIdentifier, InjectorItem> autoInjects;
    private readonly Dictionary<Type, InjectorItem>                 injectors;
    private readonly Dictionary<ReserveIdentifier, InjectorItem>    reserves;
    private readonly Dictionary<Type, List<InjectorItem>>           collections;

    private IRandom<string> randomString;

    static ReadOnlyDependencyInjectorRegister()
    {
        DefaultRandomString = RandomStringGuid.Digits;
    }

    public ReadOnlyDependencyInjectorRegister()
    {
        collections = new();
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

    public DependencyInjector Build()
    {
        var collectionItems = CreateCollections();

        return new DependencyInjector(
            injectors,
            reserves,
            autoInjects,
            collectionItems,
            randomString
        );
    }

    public void RegisterTransient(Type type, Delegate del)
    {
        if (!type.IsEnumerable())
        {
            RegisterTransientCore(type, del);

            return;
        }

        Clear(type.GenericTypeArguments[0]);
        RegisterTransientItem(type.GenericTypeArguments[0], del);
    }

    public void RegisterSingleton(Type type, Delegate del)
    {
        if (!type.IsEnumerable())
        {
            RegisterSingletonCore(type, del);

            return;
        }

        Clear(type.GenericTypeArguments[0]);
        RegisterSingletonItem(type.GenericTypeArguments[0], del);
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

    public void RegisterSingletonItem(Type type, Delegate del)
    {
        if (collections.TryGetValue(type, out var list))
        {
            list.Add(new InjectorItem(InjectorItemType.Singleton, del));

            return;
        }

        list = new List<InjectorItem> { new(InjectorItemType.Singleton, del) };
        collections.Add(type, list);
    }

    public void RegisterTransientItem(Type type, Delegate del)
    {
        if (collections.TryGetValue(type, out var list))
        {
            list.Add(new InjectorItem(InjectorItemType.Transient, del));

            return;
        }

        list = new List<InjectorItem> { new(InjectorItemType.Transient, del) };
        collections.Add(type, list);
    }

    public void Clear(Type type)
    {
        if (collections.TryGetValue(type, out var list))
        {
            list.Clear();
        }
    }

    private Dictionary<Type, IEnumerable<InjectorItem>> CreateCollections()
    {
        var result = new Dictionary<Type, IEnumerable<InjectorItem>>();

        foreach (var item in collections)
        {
            result.Add(item.Key, item.Value);
        }

        return result;
    }

    private void RegisterTransientCore(Type type, Delegate del)
    {
        injectors[type] = new InjectorItem(InjectorItemType.Transient, del);
    }

    public void RegisterSingletonCore(Type type, Delegate @delegate)
    {
        injectors[type] = new InjectorItem(InjectorItemType.Singleton, @delegate);
    }
}
