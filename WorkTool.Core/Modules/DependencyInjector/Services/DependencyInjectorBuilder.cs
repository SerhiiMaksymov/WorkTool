namespace WorkTool.Core.Modules.DependencyInjector.Services;

public class DependencyInjectorBuilder : IBuilder<IDependencyInjector>, IDependencyInjectorBuilder
{
    private readonly Dictionary<ConstructorInfo, IEnumerable<ParameterInfo>> constructorParameters;
    private readonly Dictionary<ParameterInfo, object>                       constructorParametersValue;
    private readonly Dictionary<Type, ConstructorInfo>                       constructors;
    private readonly Dictionary<Delegate, ParameterInfo[]>                   methodParameters;
    private readonly Dictionary<Delegate, MethodInfo>                        methods;
    private readonly Dictionary<Type, Func<IResolver, object>>               singleton;
    private readonly Dictionary<Type, LazyLoad<object>>                      singletonDefaults;
    private readonly Dictionary<Type, Type>                                  singletonTypes;
    private readonly Dictionary<Type, Func<IResolver, object>>               transient;
    private readonly Dictionary<Type, Func<object>>                          transientDefaults;
    private readonly Dictionary<Type, Type>                                  transientTypes;
    private readonly Dictionary<Type, IEnumerable<PropertyInfo>>             typePublicSetters;

    public bool AutoInject { get; set; }

    public DependencyInjectorBuilder()
    {
        methodParameters           = new Dictionary<Delegate, ParameterInfo[]>();
        methods                    = new Dictionary<Delegate, MethodInfo>();
        singletonDefaults          = new Dictionary<Type, LazyLoad<object>>();
        transientDefaults          = new Dictionary<Type, Func<object>>();
        constructors               = new Dictionary<Type, ConstructorInfo>();
        constructorParametersValue = new Dictionary<ParameterInfo, object>();
        constructorParameters      = new Dictionary<ConstructorInfo, IEnumerable<ParameterInfo>>();
        typePublicSetters          = new Dictionary<Type, IEnumerable<PropertyInfo>>();
        singleton                  = new Dictionary<Type, Func<IResolver, object>>();
        transient                  = new Dictionary<Type, Func<IResolver, object>>();
        singletonTypes             = new Dictionary<Type, Type>();
        transientTypes             = new Dictionary<Type, Type>();
    }

    public IDependencyInjector Build()
    {
        return new DependencyInjector(
            AutoInject,
            singletonDefaults,
            methods,
            methodParameters,
            transientDefaults,
            constructors,
            constructorParametersValue,
            constructorParameters,
            typePublicSetters,
            transient,
            singleton,
            singletonTypes,
            transientTypes);
    }

    void IRegisterTransient.RegisterTransient<TObject, TImplementation>()
    {
        RegisterTransient<TObject, TImplementation>();
    }

    void IRegisterTransient.RegisterTransient(Type type, Func<object> func)
    {
        RegisterTransient(type, func);
    }

    void IRegisterTransient.RegisterTransient(Type type, Func<IResolver, object> func)
    {
        RegisterTransient(type, func);
    }

    private IEnumerable<PropertyInfo> GetTypePublicSetters(Type type)
    {
        if (typePublicSetters.TryGetValue(type, out var properties))
        {
            return properties;
        }

        properties = type.GetProperties(BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.Public);
        typePublicSetters.Add(type, properties);

        return properties;
    }

    private IEnumerable<ParameterInfo> GetConstructorParameters(ConstructorInfo constructor)
    {
        if (constructorParameters.TryGetValue(constructor, out var parameters))
        {
            return parameters;
        }

        parameters = constructor.GetParameters();
        constructorParameters.Add(constructor, parameters);

        return parameters;
    }

    private MethodInfo GetMethodInfo(Delegate @delegate)
    {
        if (methods.ContainsKey(@delegate))
        {
            return methods[@delegate];
        }

        methods[@delegate] = @delegate.GetMethodInfo();

        return methods[@delegate];
    }

    private ParameterInfo[] GetParameterInfos(Delegate @delegate)
    {
        if (methodParameters.ContainsKey(@delegate))
        {
            return methodParameters[@delegate];
        }

        var methodInfo = GetMethodInfo(@delegate);
        methodParameters[@delegate] = methodInfo.GetParameters();

        return methodParameters[@delegate];
    }

    public DependencyInjectorBuilder Reserve(Type type, Type parameterType, object value)
    {
        var constructor = GetSingleConstructor(type);
        var parameter   = GetConstructorParameters(constructor).Single(x => x.ParameterType == parameterType);
        constructorParametersValue[parameter] = value;

        return this;
    }

    public DependencyInjectorBuilder AddConfiguration(IDependencyInjectorConfiguration configuration)
    {
        configuration.Configure(this);

        return this;
    }

    public DependencyInjectorBuilder Reserve<TObject>(Type parameterType, object value)
    {
        return Reserve(typeof(TObject), parameterType, value);
    }

    public DependencyInjectorBuilder Reserve<TObject, TParameter>(TParameter value)
    {
        return Reserve<TObject>(typeof(TParameter), value);
    }

    public DependencyInjectorBuilder Reserve<TObject>(params object[] values)
    {
        foreach (var value in values)
        {
            Reserve<TObject>(value.GetType(), value);
        }

        return this;
    }

    public DependencyInjectorBuilder RegisterTransient<TObject, TImplementation>() where TImplementation : TObject
    {
        transientTypes[typeof(TObject)] = typeof(TImplementation);

        return this;
    }

    public DependencyInjectorBuilder RegisterTransient(Type type, Func<object> func)
    {
        transientDefaults[type] = func;

        return this;
    }

    public DependencyInjectorBuilder RegisterTransient(Type type, Func<IResolver, object> func)
    {
        transient[type] = func;

        return this;
    }

    public DependencyInjectorBuilder RegisterTransient<TObject>()
    {
        return RegisterTransient<TObject, TObject>();
    }

    public DependencyInjectorBuilder RegisterTransient<TObject>(Func<TObject> func)
    {
        return RegisterTransient(typeof(TObject), () => func.Invoke());
    }

    public DependencyInjectorBuilder RegisterTransient<TObject>(Func<IResolver, TObject> func)
    {
        return RegisterTransient(typeof(TObject), r => func.Invoke(r));
    }

    public DependencyInjectorBuilder RegisterSingleton<TObject>()
    {
        singletonTypes[typeof(TObject)] = typeof(TObject);

        return this;
    }

    public DependencyInjectorBuilder RegisterSingleton(Type type, object value)
    {
        return RegisterSingleton(type, () => value);
    }

    public DependencyInjectorBuilder RegisterSingleton(object value)
    {
        return RegisterSingleton(value.GetType(), () => value);
    }

    public DependencyInjectorBuilder RegisterSingleton<TValue>(Func<TValue> func)
    {
        return RegisterSingleton(typeof(TValue), () => func.Invoke());
    }

    public DependencyInjectorBuilder RegisterSingleton<TObject, TImplementation>() where TImplementation : TObject
    {
        return RegisterSingleton(typeof(TObject), typeof(TImplementation));
    }

    public DependencyInjectorBuilder RegisterSingleton<TObject>(object value)
    {
        return RegisterSingleton(typeof(TObject), value);
    }

    public DependencyInjectorBuilder RegisterSingleton(Type type, Func<object> func)
    {
        return RegisterSingleton(type, new LazyLoad<object>(func));
    }

    public DependencyInjectorBuilder RegisterSingleton(Type type, LazyLoad<object> lazy)
    {
        if (transientDefaults.ContainsKey(type))
        {
            transientDefaults.Remove(type);
        }

        singletonDefaults[type] = lazy;

        return this;
    }

    private ConstructorInfo GetSingleConstructor(Type type)
    {
        if (this.constructors.TryGetValue(type, out var constructor))
        {
            return constructor;
        }

        var constructors = type.GetConstructors();

        if (constructors.Length == 0)
        {
            throw new Exception($"Can't find constructor for {type}.");
        }

        if (constructors.Length != 1)
        {
            throw new Exception($"Can't chose constructor for {type}.");
        }

        constructor = constructors[0];
        this.constructors.Add(type, constructor);

        return constructor;
    }
}