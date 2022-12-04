namespace WorkTool.Core.Modules.DependencyInjector.Services;

public class DependencyInjector : IDependencyInjector
{
    private readonly bool                                                    autoInject;
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

    public DependencyInjector(
    bool                                                             autoInject,
    IReadOnlyDictionary<Type, LazyLoad<object>>                      singletonDefaults,
    IReadOnlyDictionary<Delegate, MethodInfo>                        methods,
    IReadOnlyDictionary<Delegate, ParameterInfo[]>                   methodParameters,
    IReadOnlyDictionary<Type, Func<object>>                          transientDefaults,
    IReadOnlyDictionary<Type, ConstructorInfo>                       constructors,
    IReadOnlyDictionary<ParameterInfo, object>                       constructorParametersValue,
    IReadOnlyDictionary<ConstructorInfo, IEnumerable<ParameterInfo>> constructorParameters,
    IReadOnlyDictionary<Type, IEnumerable<PropertyInfo>>             typePublicSetters,
    IReadOnlyDictionary<Type, Func<IResolver, object>>               transient,
    IReadOnlyDictionary<Type, Func<IResolver, object>>               singleton,
    IReadOnlyDictionary<Type, Type>                                  singletonTypes,
    IReadOnlyDictionary<Type, Type>                                  transientTypes)
    {
        this.autoInject = autoInject;
        this.singletonTypes = new Dictionary<Type, Type>(singletonTypes);
        this.transientTypes = new Dictionary<Type, Type>(transientTypes);
        this.transientDefaults = new Dictionary<Type, Func<object>>(transientDefaults);
        this.singletonDefaults = new Dictionary<Type, LazyLoad<object>>(singletonDefaults);
        this.methodParameters = new Dictionary<Delegate, ParameterInfo[]>(methodParameters);
        this.methods = new Dictionary<Delegate, MethodInfo>(methods);
        this.singleton = new Dictionary<Type, Func<IResolver, object>>(singleton);
        this.transient = new Dictionary<Type, Func<IResolver, object>>(transient);
        this.constructors = new Dictionary<Type, ConstructorInfo>(constructors);
        this.constructorParametersValue = new Dictionary<ParameterInfo, object>(constructorParametersValue);
        this.constructorParameters = new Dictionary<ConstructorInfo, IEnumerable<ParameterInfo>>(constructorParameters);
        this.typePublicSetters = new Dictionary<Type, IEnumerable<PropertyInfo>>(typePublicSetters);
    }

    public object Invoke(Delegate @delegate, IEnumerable<ArgumentValue> arguments)
    {
        var argumentsInvoke = GetArgumentsInvoke(@delegate, arguments);

        return @delegate.DynamicInvoke(argumentsInvoke);
    }

    public TResult Invoke<TResult>(Delegate @delegate, IEnumerable<ArgumentValue> arguments)
    {
        var argumentsInvoke = GetArgumentsInvoke(@delegate, arguments);

        return (TResult)@delegate.DynamicInvoke(argumentsInvoke);
    }

    public Task InvokeAsync(Delegate @delegate, IEnumerable<ArgumentValue> arguments)
    {
        var argumentsInvoke = GetArgumentsInvoke(@delegate, arguments);

        return (Task)@delegate.DynamicInvoke(argumentsInvoke);
    }

    public Task<TResult> InvokeAsync<TResult>(Delegate @delegate, IEnumerable<ArgumentValue> arguments)
    {
        var argumentsInvoke = GetArgumentsInvoke(@delegate, arguments);

        return (Task<TResult>)@delegate.DynamicInvoke(argumentsInvoke);
    }

    public object Resolve(Type type)
    {
        if (singletonDefaults.TryGetValue(type, out var lazySingletonDefaultFunc))
        {
            var value = lazySingletonDefaultFunc.GetValue();

            return value;
        }

        if (transientDefaults.TryGetValue(type, out var transientDefaultFunc))
        {
            var value = transientDefaultFunc.Invoke();

            return value;
        }

        if (singleton.TryGetValue(type, out var lazySingletonFunc))
        {
            var value = lazySingletonFunc.Invoke(this);
            singletonDefaults.Add(type, new LazyLoad<object>(() => value));

            return value;
        }

        if (transient.TryGetValue(type, out var transientFunc))
        {
            var value = transientFunc.Invoke(this);

            return value;
        }

        if (singletonTypes.TryGetValue(type, out var singletonTypeImp))
        {
            var value = Create(singletonTypeImp);
            singletonDefaults.Add(type, new LazyLoad<object>(() => value));

            return value;
        }

        if (transientTypes.TryGetValue(type, out var transientTypeImp))
        {
            var value = Create(transientTypeImp);
            transientDefaults[type] = () => value;

            return value;
        }

        return Create(type);
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

    private object[] GetArgumentsInvoke(Delegate @delegate, IEnumerable<ArgumentValue> arguments)
    {
        var parameters = GetParameterInfos(@delegate);

        if (parameters.IsEmpty())
        {
            return Array.Empty<object>();
        }

        var argumentsInvokeLenght = parameters.Length;
        var parameterIndex        = 0;

        if (parameters[0].ParameterType.FullName.Equals("System.Runtime.CompilerServices.Closure"))
        {
            argumentsInvokeLenght--;
            parameterIndex++;
        }

        if (argumentsInvokeLenght == 0)
        {
            return Array.Empty<object>();
        }

        var argumentsInvokeIndex = 0;
        var argumentsInvoke      = new object[argumentsInvokeLenght];
        var argumentsDictionary  = new Dictionary<Type, object>();

        foreach (var argument in arguments)
        {
            argumentsDictionary[argument.Type] = argument.Value;
        }

        for (; parameterIndex < parameters.Length; parameterIndex++, argumentsInvokeIndex++)
        {
            if (argumentsDictionary.ContainsKey(parameters[parameterIndex].ParameterType))
            {
                argumentsInvoke[argumentsInvokeIndex] = argumentsDictionary[parameters[parameterIndex].ParameterType];

                continue;
            }

            argumentsInvoke[argumentsInvokeIndex] = Resolve(parameters[parameterIndex].ParameterType);
        }

        return argumentsInvoke;
    }

    private object CreateByConstructor(ConstructorInfo constructor)
    {
        var parameters      = GetConstructorParameters(constructor);
        var parameterValues = parameters.Select(x => GetParameterValue(x)).ToArray();
        var result          = Activator.CreateInstance(constructor.DeclaringType, parameterValues);

        return result;
    }

    private object GetParameterValue(ParameterInfo parameter)
    {
        if (constructorParametersValue.TryGetValue(parameter, out var value))
        {
            return value;
        }

        return Resolve(parameter.ParameterType);
    }

    public void Inject(object value)
    {
        var properties = GetTypePublicSetters(value.GetType());

        foreach (var property in properties)
        {
            if (autoInject)
            {
                if (!property.GetCustomAttributes<NonInjectAttribute>().Any())
                {
                    property.SetValue(value, Resolve(property.PropertyType));
                }
            }
            else
            {
                if (property.GetCustomAttributes<InjectAttribute>().Any())
                {
                    property.SetValue(value, Resolve(property.PropertyType));
                }
            }
        }
    }

    private object Create(Type type)
    {
        var constructor = GetSingleConstructor(type);

        return CreateByConstructor(constructor);
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