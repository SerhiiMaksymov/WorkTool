namespace WorkTool.Core.Modules.DependencyInjection.Services;

public class MutDependencyInjector : IMutDependencyInjector
{
    private readonly Dictionary<AutoInjectIdentifier, InjectorItem> autoInjects;
    private readonly Dictionary<Type, InjectorItem> injectors;
    private readonly Dictionary<ReserveIdentifier, InjectorItem> reserves;
    private readonly Dictionary<Type, object> singletonInjectors;
    private readonly Dictionary<ReserveIdentifier, object> singletonReserves;
    private readonly Dictionary<AutoInjectIdentifier, object> singletonAutoInjects;

    public MutDependencyInjector()
    {
        var injectorItem = new InjectorItem(InjectorItemType.Singleton, () => this);
        autoInjects = new();
        reserves = new();
        singletonInjectors = new() { { typeof(IResolver), this }, { typeof(IInvoker), this } };
        singletonReserves = new();
        singletonAutoInjects = new();

        injectors = new()
        {
            { typeof(IResolver), injectorItem },
            { typeof(IInvoker), injectorItem }
        };
    }

    public object Resolve(Type type)
    {
        if (injectors.ContainsKey(type))
        {
            var injector = injectors[type];

            switch (injector.Type)
            {
                case InjectorItemType.Singleton:
                {
                    if (singletonInjectors.ContainsKey(type))
                    {
                        return singletonInjectors[type];
                    }

                    var obj = Create(injector.Delegate);
                    singletonInjectors[type] = obj;

                    return obj;
                }
                case InjectorItemType.Transient:
                {
                    var obj = Create(injector.Delegate);

                    return obj;
                }
                default:
                    throw new UnreachableException();
            }
        }

        return CreateObject(type);
    }

    private object Create(Delegate del)
    {
        var parameters = del.Method.GetParameters();

        if (parameters.Length == 1 && parameters[0].ParameterType == del.Method.ReturnType)
        {
            var obj = CreateObject(del.Method.ReturnType);
            AutoInject(obj);

            return obj;
        }

        var parameterValues = new List<object>();

        foreach (var parameter in parameters)
        {
            parameterValues.Add(Resolve(parameter.ParameterType));
        }

        var result = del.DynamicInvoke(parameterValues.ToArray()).ThrowIfNull();
        AutoInject(result);

        return result;
    }

    private object AutoInject(object obj)
    {
        var members = obj.GetType().GetMembers();

        foreach (var member in members)
        {
            switch (member)
            {
                case PropertyInfo property:
                {
                    if (!property.CanWrite)
                    {
                        break;
                    }

                    var identifier = new AutoInjectIdentifier(obj.GetType(), member);

                    if (!autoInjects.TryGetValue(identifier, out var item))
                    {
                        continue;
                    }

                    var value = GetAutoInjectValue(identifier, item);
                    property.SetValue(obj, value);

                    break;
                }
                case FieldInfo field:
                {
                    if (field.IsInitOnly)
                    {
                        break;
                    }

                    var identifier = new AutoInjectIdentifier(obj.GetType(), member);

                    if (!autoInjects.TryGetValue(identifier, out var item))
                    {
                        continue;
                    }

                    var value = GetAutoInjectValue(identifier, item);
                    field.SetValue(obj, value);

                    break;
                }
            }
        }

        return obj;
    }

    private object GetAutoInjectValue(AutoInjectIdentifier identifier, InjectorItem item)
    {
        var parameters = item.Delegate.Method.GetParameters();

        if (
            parameters.Length == 1 && parameters[0].ParameterType == item.Delegate.Method.ReturnType
        )
        {
            var obj = Resolve(parameters[0].ParameterType);
            AutoInject(obj);

            return obj;
        }

        switch (item.Type)
        {
            case InjectorItemType.Singleton:
            {
                if (singletonAutoInjects.TryGetValue(identifier, out var value))
                {
                    return value;
                }

                var obj = Create(item.Delegate);
                singletonAutoInjects[identifier] = obj;

                return obj;
            }
            case InjectorItemType.Transient:
            {
                var obj = Create(item.Delegate);

                return obj;
            }
            default:
                throw new UnreachableException();
        }
    }

    private ConstructorInfo? GetSingleConstructor(Type type)
    {
        var constructors = type.GetConstructors();

        if (constructors.Length == 0)
        {
            return null;
        }

        if (constructors.Length > 1)
        {
            throw new ToManyConstructorsException(type, 1, constructors.Length);
        }

        return constructors[0];
    }

    private object CreateObject(Type type)
    {
        var constructor = GetSingleConstructor(type);

        if (constructor is null && type.IsValueType)
        {
            var obj = Activator.CreateInstance(type).ThrowIfNull();
            AutoInject(obj);

            return obj;
        }

        var parameters = constructor.GetParameters();
        var parameterValues = new List<object>();

        foreach (var parameter in parameters)
        {
            var identifier = new ReserveIdentifier(type, parameter);

            if (!reserves.TryGetValue(identifier, out var value))
            {
                parameterValues.Add(Resolve(parameter.ParameterType));

                continue;
            }

            switch (value.Type)
            {
                case InjectorItemType.Singleton:
                {
                    if (singletonReserves.TryGetValue(identifier, out var reserve))
                    {
                        parameterValues.Add(reserve);
                    }
                    else
                    {
                        var obj = Create(value.Delegate);
                        singletonReserves[identifier] = obj;
                        parameterValues.Add(obj);
                    }

                    break;
                }
                case InjectorItemType.Transient:
                {
                    var obj = Create(value.Delegate);
                    parameterValues.Add(obj);

                    break;
                }
                default:
                    throw new UnreachableException();
            }
        }

        var result = Activator.CreateInstance(type, parameterValues.ToArray()).ThrowIfNull();
        AutoInject(result);

        return result;
    }

    public object? Invoke(Delegate del, DictionarySpan<Type, object> arguments)
    {
        var parametersValues = new List<object>();
        var parameters = del.Method.GetParameters();

        foreach (var parameter in parameters)
        {
            if (arguments.TryGetValue(parameter.ParameterType, out var value))
            {
                parametersValues.Add(value);
            }
            else
            {
                parametersValues.Add(Resolve(parameter.ParameterType));
            }
        }

        return del.DynamicInvoke(parametersValues.ToArray());
    }

    public void RegisterTransient(Type type, Delegate del)
    {
        injectors[type] = new InjectorItem(InjectorItemType.Transient, del);

        if (singletonInjectors.ContainsKey(type))
        {
            singletonInjectors.Remove(type);
        }
    }

    public void RegisterSingleton(Type type, Delegate del)
    {
        injectors[type] = new InjectorItem(InjectorItemType.Singleton, del);

        if (singletonInjectors.ContainsKey(type))
        {
            singletonInjectors.Remove(type);
        }
    }

    public void RegisterReserveSingleton(ReserveIdentifier identifier, Delegate del)
    {
        reserves[identifier] = new InjectorItem(InjectorItemType.Singleton, del);

        if (singletonReserves.ContainsKey(identifier))
        {
            singletonReserves.Remove(identifier);
        }
    }

    public void RegisterReserveTransient(ReserveIdentifier identifier, Delegate del)
    {
        reserves[identifier] = new InjectorItem(InjectorItemType.Transient, del);

        if (singletonReserves.ContainsKey(identifier))
        {
            singletonReserves.Remove(identifier);
        }
    }

    public void RegisterTransientAutoInject(AutoInjectIdentifier identifier, Delegate del)
    {
        autoInjects[identifier] = new InjectorItem(InjectorItemType.Transient, del);

        if (singletonAutoInjects.ContainsKey(identifier))
        {
            singletonAutoInjects.Remove(identifier);
        }
    }

    public void RegisterSingletonAutoInject(AutoInjectIdentifier identifier, Delegate del)
    {
        autoInjects[identifier] = new InjectorItem(InjectorItemType.Singleton, del);

        if (singletonAutoInjects.ContainsKey(identifier))
        {
            singletonAutoInjects.Remove(identifier);
        }
    }

    public void RegisterConfiguration(IDependencyInjectorConfiguration configuration)
    {
        configuration.Configure(this);
    }
}
