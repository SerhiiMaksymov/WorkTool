namespace WorkTool.Core.Modules.DependencyInjection.Models;

public readonly struct DependencyInjectorFields
{
    public readonly Dictionary<AutoInjectIdentifier, InjectorItem> AutoInjects;
    public readonly Dictionary<TypeInformation, InjectorItem> Injectors;
    public readonly Dictionary<TypeInformation, object> CacheSingletonValues;
    public readonly Dictionary<TypeInformation, Func<object>> CacheTransientValues;
    public readonly Dictionary<TypeInformation, Expression> CacheExpressions;
    public readonly Dictionary<TypeInformation, IEnumerable<InjectorItem>> Collections;
    public readonly IRandom<string> RandomString;
    public readonly ReadOnlyMemory<TypeInformation> Inputs;
    public readonly ReadOnlyMemory<TypeInformation> Outputs;

    public DependencyInjectorFields(
        IReadOnlyDictionary<TypeInformation, InjectorItem> injectors,
        IReadOnlyDictionary<AutoInjectIdentifier, InjectorItem> autoInjects,
        IReadOnlyDictionary<TypeInformation, IEnumerable<InjectorItem>> collections,
        IRandom<string> randomString,
        InjectorItem resolver,
        InjectorItem invoker
    )
    {
        RandomString = randomString;
        Injectors = new(injectors);
        Collections = new(collections);
        AutoInjects = new(autoInjects);
        CacheSingletonValues = new();
        CacheExpressions = new();
        CacheTransientValues = new();
        var enumerableType = typeof(IEnumerable<>);

        if (!Injectors.ContainsKey(typeof(IResolver)))
        {
            Injectors.Add(typeof(IResolver), resolver);
        }

        if (!Injectors.ContainsKey(typeof(IInvoker)))
        {
            Injectors.Add(typeof(IInvoker), invoker);
        }

        var array = Injectors
            .Select(x => x.Key)
            .Concat(
                Collections.Select(x => (TypeInformation)enumerableType.MakeGenericType(x.Key.Type))
            )
            .ToArray();

        Outputs = array;

        Inputs = GetInputs(Injectors, AutoInjects)
            .Distinct()
            .Where(x => !array.Contains(x) && !x.Type.IsClosure())
            .ToArray();
    }

    private IEnumerable<TypeInformation> GetInputs(
        IReadOnlyDictionary<TypeInformation, InjectorItem> injectors,
        IReadOnlyDictionary<AutoInjectIdentifier, InjectorItem> autoInjects
    )
    {
        foreach (var value in GetInputs(injectors))
        {
            yield return value;
        }

        foreach (var value in GetInputs(autoInjects))
        {
            yield return value;
        }
    }

    private IEnumerable<TypeInformation> GetInputs(
        IReadOnlyDictionary<AutoInjectIdentifier, InjectorItem> autoInjects
    )
    {
        foreach (var autoInject in autoInjects)
        {
            foreach (var value in GetInputs(autoInject.Value.Delegate))
            {
                yield return value;
            }
        }
    }

    private IEnumerable<TypeInformation> GetInputs(
        IReadOnlyDictionary<TypeInformation, InjectorItem> injectors
    )
    {
        foreach (var injector in injectors)
        {
            foreach (var value in GetInputs(injector.Value.Delegate))
            {
                yield return value;
            }
        }
    }

    private IEnumerable<TypeInformation> GetInputs(Delegate del)
    {
        foreach (var injector in del.GetParameterTypes())
        {
            yield return injector;
        }
    }
}
