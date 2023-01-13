namespace WorkTool.Core.Modules.DependencyInjection.Services;

public class DependencyInjector : IDependencyInjector
{
    private readonly Dictionary<ReserveIdentifier, InjectorItem>    reserves;
    private readonly Dictionary<ReserveIdentifier, Expression>      cacheReservesExpressions;
    private readonly Dictionary<AutoInjectIdentifier, InjectorItem> autoInjects;
    private readonly Dictionary<Type, InjectorItem>                 injectors;
    private readonly Dictionary<Type, object>                       cacheSingletonValues;
    private readonly Dictionary<Type, Func<object>>                 cacheTransientValues;
    private readonly Dictionary<Type, Expression>                   cacheExpressions;
    private readonly Dictionary<Type, IEnumerable<InjectorItem>>    collections;
    private readonly IRandom<string>                                randomString;

    public DependencyInjector(
        IReadOnlyDictionary<Type, InjectorItem>                 injectors,
        IReadOnlyDictionary<ReserveIdentifier, InjectorItem>    reserves,
        IReadOnlyDictionary<AutoInjectIdentifier, InjectorItem> autoInjects,
        IReadOnlyDictionary<Type, IEnumerable<InjectorItem>>    collections,
        IRandom<string>                                         randomString
    )
    {
        this.randomString = randomString;
        this.collections = new(collections);
        this.autoInjects = new(autoInjects);
        this.injectors = new(injectors);
        this.reserves = new(reserves);
        cacheSingletonValues = new() { { typeof(IResolver), this }, { typeof(IInvoker), this } };
        cacheTransientValues = new();
        cacheReservesExpressions = new();
        var enumerableType = typeof(IEnumerable<>);

        Outputs = this.injectors
            .Select(x => x.Key)
            .Concat(this.collections.Select(x => enumerableType.MakeGenericType(x.Key)))
            .ToArray();

        Inputs = this.injectors
            .SelectMany(x => x.Value.Delegate.GetParameterTypes())
            .Concat(this.autoInjects.SelectMany(x => x.Value.Delegate.GetParameterTypes()))
            .Distinct()
            .Where(x => !Outputs.Contains(x))
            .ToArray();

        cacheExpressions = new()
        {
            { typeof(IResolver), this.ToConstant() },
            { typeof(IInvoker), this.ToConstant() }
        };
    }

    public IEnumerable<Type> Inputs  { get; }
    public IEnumerable<Type> Outputs { get; }

    public object Resolve(Type type)
    {
        var cache = GetCacheValue(type);

        if (cache is not null)
        {
            return cache;
        }

        CacheValue(type);
        var result = GetCacheValue(type).ThrowIfNull();

        return result;
    }

#region AutoInject

    private Expression GetAutoInjectExpression(Type type, Expression root)
    {
        var expressions = GetTypeMembersExpressions(type);

        if (expressions.IsEmpty())
        {
            return root;
        }

        var id = randomString.GetRandom();
        var variable = root.Type.ToVariable($"newInstance{id}");

        var blockItems = new List<Expression> { variable.Assign(root) };

        foreach (var expression in expressions)
        {
            var memberExpression = Expression.PropertyOrField(variable, expression.Member.Name);
            blockItems.Add(memberExpression);
            blockItems.Add(memberExpression.Assign(expression.Expression));
        }

        blockItems.Add(variable);

        return Expression.Block(new[] { variable }, blockItems);
    }

    private List<(MemberInfo Member, Expression Expression)> GetTypeMembersExpressions(Type type)
    {
        var list = new List<(MemberInfo Member, Expression Expression)>();

        foreach (var member in GetAutoInjectMembers(type))
        {
            list.Add((member, GetAutoInjectsExpression(type, member)));
        }

        return list;
    }

    private Expression GetAutoInjectsExpression(Type type, MemberInfo member)
    {
        var autoInjectIdentifier = new AutoInjectIdentifier(type, member);
        var injectorItem = autoInjects[autoInjectIdentifier];

        switch (injectorItem.Type)
        {
            case InjectorItemType.Singleton:
            {
                return GetSingletonAutoInjectsValue(injectorItem.Delegate, member);
            }
            case InjectorItemType.Transient:
            {
                return GetTransientAutoInjectsValue(injectorItem.Delegate, member);
            }
            default:
            {
                throw new UnreachableException();
            }
        }
    }

    private Expression GetSingletonAutoInjectsValue(Delegate del, MemberInfo member)
    {
        var parameters = del.Method.GetParameters();

        if (parameters.Length == 1 && parameters[0].ParameterType == del.Method.ReturnType)
        {
            return GetOrCacheExpression(del.Method.ReturnType);
        }

        var expressions = ConstructorParametersToExpressions(
            member.DeclaringType.ThrowIfNull(),
            parameters
        );

        var instance = del.Target.ThrowIfNull().ToConstant();

        var constant = del.Method
            .ToCall(instance, expressions)
            .Lambda()
            .Compile()
            .DynamicInvoke()
            .ThrowIfNull()
            .ToConstant();

        return constant;
    }

    private Expression GetTransientAutoInjectsValue(Delegate del, MemberInfo member)
    {
        var parameters = del.Method.GetParameters();

        if (parameters.Length == 1 && parameters[0].ParameterType == del.Method.ReturnType)
        {
            return GetOrCacheExpression(del.Method.ReturnType);
        }

        var expressions = ConstructorParametersToExpressions(
            member.DeclaringType.ThrowIfNull(),
            parameters
        );

        var instance = del.Target.ThrowIfNull().ToConstant();
        var call = del.Method.ToCall(instance, expressions);

        return call;
    }

    private List<MemberInfo> GetAutoInjectMembers(Type type)
    {
        var result = new List<MemberInfo>();

        var members = type.GetMembers()
            .Where(x => x is PropertyInfo { CanWrite: true } or FieldInfo { IsInitOnly: false });

        foreach (var member in members)
        {
            var autoInjectIdentifier = new AutoInjectIdentifier(type, member);

            if (!autoInjects.ContainsKey(autoInjectIdentifier))
            {
                continue;
            }

            result.Add(member);
        }

        return result;
    }

#endregion

#region Injector

    private List<Expression> ConstructorParametersToExpressions(
    Type                           type,
        IEnumerable<ParameterInfo> parameters
    )
    {
        var result = new List<Expression>();

        foreach (var parameter in parameters)
        {
            CacheReserveValue(type, parameter);
            var expression = GetOrCacheExpression(type, parameter);

            if (expression.Type.IsValueType && !parameter.ParameterType.IsValueType)
            {
                expression = expression.Convert(parameter.ParameterType);
            }

            result.Add(expression);
        }

        return result;
    }

    private Expression GetOrCacheExpression(Type type)
    {
        var expression = GetCacheExpression(type);

        if (expression is null)
        {
            CacheValue(type);
            expression = GetCacheExpression(type).ThrowIfNull();
        }

        return expression;
    }

    private Expression GetOrCacheExpression(Type type, ParameterInfo parameter)
    {
        var expression = GetCacheExpression(type, parameter);

        if (expression is null)
        {
            CacheValue(parameter.ParameterType);
            expression = GetCacheExpression(type, parameter).ThrowIfNull();
        }

        return expression;
    }

    private Expression? GetCacheExpression(Type parentType, ParameterInfo parameter)
    {
        var reserveIdentifier = new ReserveIdentifier(parentType, parameter);

        if (cacheReservesExpressions.TryGetValue(reserveIdentifier, out var expression))
        {
            return expression;
        }

        return GetCacheExpression(parameter.ParameterType);
    }

    private Expression? GetCacheExpression(Type type)
    {
        if (cacheExpressions.TryGetValue(type, out var expression))
        {
            return expression;
        }

        return null;
    }

    private object? GetCacheValue(Type type)
    {
        if (cacheSingletonValues.TryGetValue(type, out var singletonValue))
        {
            return singletonValue;
        }

        if (cacheTransientValues.TryGetValue(type, out var transientValue))
        {
            var value = transientValue.Invoke();

            return value;
        }

        return null;
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

    private void CacheCollection(Type typeEnumerable)
    {
        var type = typeEnumerable.GenericTypeArguments[0];

        if (collections.TryGetValue(type, out var list))
        {
            CacheCollection(typeEnumerable, type, list);

            return;
        }

        var expression = type.ToNewArrayInit();

        var value = expression
            .Convert(typeof(object))
            .Lambda()
            .Compile()
            .ThrowIfIsNot<Func<object>>()
            .Invoke();

        cacheExpressions.Add(typeEnumerable, value.ToConstant());
        cacheSingletonValues.Add(typeEnumerable, value);
    }

    private IEnumerable<Expression> CreateExpressions(Type type, IEnumerable<InjectorItem> items)
    {
        foreach (var item in items)
        {
            switch (item.Type)
            {
                case InjectorItemType.Singleton:
                {
                    yield return CreateSingletonValue(type, item);

                    break;
                }
                case InjectorItemType.Transient:
                {
                    yield return CreateTransientValue(type, item);

                    break;
                }
                default:
                {
                    throw new UnreachableException();
                }
            }
        }
    }

    private void CacheCollection(Type typeEnumerable, Type type, IEnumerable<InjectorItem> items)
    {
        var listType = typeof(List<>).MakeGenericType(type);
        var listAddMethod = listType.GetMethod(nameof(List<object>.Add)).ThrowIfNull();
        var listAddRangeMethod = listType.GetMethod(nameof(List<object>.AddRange)).ThrowIfNull();
        var id = randomString.GetRandom();
        var variable = listType.ToVariable($"newInstance{id}");
        var listExpressions = CreateExpressions(type, items).ToArray();
        var blockItems = new List<Expression> { variable.Assign(listType.ToNew()) };

        foreach (var item in listExpressions)
        {
            if (typeEnumerable.IsAssignableFrom(item.Type))
            {
                blockItems.Add(listAddRangeMethod.ToCall(variable, item.Convert(typeEnumerable)));
            }
            else
            {
                blockItems.Add(listAddMethod.ToCall(variable, item.Convert(type)));
            }
        }

        blockItems.Add(variable);
        var expression = Expression.Block(new[] { variable }, blockItems);
        CacheTransient(typeEnumerable, expression);
    }

    private void CacheValue(Type type)
    {
        if (type.IsEnumerable())
        {
            CacheCollection(type);

            return;
        }

        if (injectors.TryGetValue(type, out var injectorItem))
        {
            switch (injectorItem.Type)
            {
                case InjectorItemType.Singleton:
                {
                    CacheSingleton(type, CreateSingletonValue(type));

                    return;
                }
                case InjectorItemType.Transient:
                {
                    CacheTransient(type, CreateTransientValue(type));

                    return;
                }
                default:
                {
                    throw new UnreachableException();
                }
            }
        }

        CacheTransient(type, CreateTransientValue(type));
    }

    private Expression CreateTransientValue(Type type)
    {
        if (!injectors.ContainsKey(type))
        {
            return CreateTransientDefaultValue(type);
        }

        var injector = injectors[type];

        return CreateTransientValue(type, injector);
    }

    private Expression CreateTransientValue(Type type, InjectorItem injector)
    {
        var parameters = injector.Delegate.Method.GetParameters();

        if (parameters.Length == 1 && parameters[0].ParameterType == type)
        {
            return CreateTransientDefaultValue(type);
        }

        var expressions = ConstructorParametersToExpressions(type, parameters);
        var instance = injector.Delegate.Target.ThrowIfNull().ToConstant();
        var newExpression = injector.Delegate.Method.ToCall(instance, expressions);
        var result = GetAutoInjectExpression(type, newExpression);

        return result;
    }

    private Expression CreateTransientDefaultValue(Type type)
    {
        var constructor = GetSingleConstructor(type);

        if (constructor is null)
        {
            return CreateTransientValueTypeValue(type);
        }

        var parameters = constructor.GetParameters();
        var expressions = ConstructorParametersToExpressions(type, parameters);
        var newExpression = constructor.ToNew(expressions);
        var result = GetAutoInjectExpression(type, newExpression);

        return result;
    }

    private Expression CreateTransientValueTypeValue(Type type)
    {
        if (!type.IsValueType)
        {
            throw new NotHaveConstructorException(type);
        }

        var obj = type.ToNew().ThrowIfNull();
        var result = GetAutoInjectExpression(type, obj);

        return result;
    }

    private void CacheTransient(Type type, Expression expression)
    {
        var transientValue = expression
            .Convert(typeof(object))
            .Lambda()
            .Compile()
            .ThrowIfIsNot<Func<object>>();

        cacheExpressions.Add(type, expression);
        cacheTransientValues.Add(type, transientValue);
    }

    private Expression CreateSingletonValue(Type type)
    {
        if (!injectors.ContainsKey(type))
        {
            return CreateSingletonDefaultValue(type);
        }

        var injector = injectors[type];

        return CreateSingletonValue(type, injector);
    }

    private Expression CreateSingletonValue(Type type, InjectorItem injector)
    {
        var parameters = injector.Delegate.Method.GetParameters();

        if (parameters.Length == 1 && parameters[0].ParameterType == type)
        {
            return CreateSingletonDefaultValue(type);
        }

        var expressions = ConstructorParametersToExpressions(type, parameters);

        var newExpression = injector.Delegate.Method
            .ToCall(injector.Delegate.Target.ThrowIfNull().ToConstant(), expressions)
            .Lambda()
            .Compile();

        var obj = newExpression.DynamicInvoke().ThrowIfNull();
        var result = GetAutoInjectExpression(type, obj.ToConstant());

        return result;
    }

    private Expression CreateSingletonValueTypeValue(Type type)
    {
        if (!type.IsValueType)
        {
            throw new NotHaveConstructorException(type);
        }

        var obj = Activator.CreateInstance(type).ThrowIfNull();
        var result = GetAutoInjectExpression(type, obj.ToConstant());

        return result;
    }

    private void CacheSingleton(Type type, Expression expression)
    {
        var value = expression.Lambda().Compile().DynamicInvoke().ThrowIfNull();
        cacheExpressions.Add(type, value.ToConstant());
        cacheSingletonValues.Add(type, value);
    }

    private Expression CreateSingletonDefaultValue(Type type)
    {
        var constructor = GetSingleConstructor(type);

        if (constructor is null)
        {
            return CreateSingletonValueTypeValue(type);
        }

        var parameters = constructor.GetParameters();
        var expressions = ConstructorParametersToExpressions(type, parameters);
        var newExpression = constructor.ToNew(expressions).Lambda().Compile();
        var obj = newExpression.DynamicInvoke().ThrowIfNull();
        var result = GetAutoInjectExpression(type, obj.ToConstant());

        return result;
    }

#endregion

#region Reserve

    private void CacheReserveValue(Type type, ParameterInfo parameter)
    {
        var reserveIdentifier = new ReserveIdentifier(type, parameter);

        if (!reserves.TryGetValue(reserveIdentifier, out var injectorItem))
        {
            return;
        }

        switch (injectorItem.Type)
        {
            case InjectorItemType.Singleton:
            {
                CacheSingletonReserveValue(type, parameter, injectorItem.Delegate);

                break;
            }
            case InjectorItemType.Transient:
            {
                CacheTransientReserveValue(type, parameter, injectorItem.Delegate);

                break;
            }
            default:
            {
                throw new UnreachableException();
            }
        }
    }

    private void CacheTransientValueTypeReserveValue(Type type, ParameterInfo parameter)
    {
        if (!parameter.ParameterType.IsValueType)
        {
            throw new NotHaveConstructorException(type);
        }

        var reserveIdentifier = new ReserveIdentifier(type, parameter);
        var obj = parameter.ParameterType.ToNew().ThrowIfNull();
        cacheReservesExpressions.Add(reserveIdentifier, obj);
    }

    private void CacheTransientDefaultReserveValue(Type type, ParameterInfo parameter)
    {
        var constructor = GetSingleConstructor(parameter.ParameterType);

        if (constructor is null)
        {
            CacheTransientValueTypeReserveValue(type, parameter);

            return;
        }

        var reserveIdentifier = new ReserveIdentifier(type, parameter);
        var parameters = constructor.GetParameters();
        var expressions = ConstructorParametersToExpressions(parameter.ParameterType, parameters);
        var newExpression = constructor.ToNew(expressions);
        cacheReservesExpressions.Add(reserveIdentifier, newExpression);
    }

    private void CacheTransientReserveValue(Type type, ParameterInfo parameter, Delegate del)
    {
        var parameters = del.Method.GetParameters();

        if (parameters.Length == 1 && parameters[0].ParameterType == parameter.ParameterType)
        {
            CacheTransientDefaultReserveValue(type, parameter);

            return;
        }

        var reserveIdentifier = new ReserveIdentifier(type, parameter);
        var expressions = ConstructorParametersToExpressions(parameter.ParameterType, parameters);
        var instance = del.Target.ThrowIfNull().ToConstant();
        var newExpression = del.Method.ToCall(instance, expressions);
        cacheReservesExpressions.Add(reserveIdentifier, newExpression);
    }

    private void CacheSingletonValueTypeReserveValue(Type type, ParameterInfo parameter)
    {
        if (!parameter.ParameterType.IsValueType)
        {
            throw new NotHaveConstructorException(type);
        }

        var reserveIdentifier = new ReserveIdentifier(type, parameter);
        var obj = Activator.CreateInstance(parameter.ParameterType).ThrowIfNull();
        cacheReservesExpressions.Add(reserveIdentifier, obj.ToConstant(parameter.ParameterType));
    }

    private void CacheSingletonDefaultReserveValue(Type type, ParameterInfo parameter)
    {
        var constructor = GetSingleConstructor(parameter.ParameterType);

        if (constructor is null)
        {
            CacheSingletonValueTypeReserveValue(type, parameter);

            return;
        }

        var reserveIdentifier = new ReserveIdentifier(type, parameter);
        var parameters = constructor.GetParameters();
        var expressions = ConstructorParametersToExpressions(parameter.ParameterType, parameters);
        var newExpression = constructor.ToNew(expressions).Lambda().Compile();
        var obj = newExpression.DynamicInvoke().ThrowIfNull();
        cacheReservesExpressions.Add(reserveIdentifier, obj.ToConstant());
    }

    private void CacheSingletonReserveValue(Type type, ParameterInfo parameter, Delegate del)
    {
        var parameters = del.Method.GetParameters();

        if (parameters.Length == 1 && parameters[0].ParameterType == parameter.ParameterType)
        {
            CacheSingletonDefaultReserveValue(type, parameter);

            return;
        }

        var reserveIdentifier = new ReserveIdentifier(type, parameter);
        var expressions = ConstructorParametersToExpressions(parameter.ParameterType, parameters);

        var newExpression = del.Method
            .ToCall(del.Target.ThrowIfNull().ToConstant(), expressions)
            .Lambda()
            .Compile();

        var obj = newExpression.DynamicInvoke().ThrowIfNull();
        cacheReservesExpressions.Add(reserveIdentifier, obj.ToConstant());
    }

#endregion

    public object? Invoke(Delegate del, DictionarySpan<Type, object> arguments)
    {
        var parameterTypes = del.GetParameterTypes();
        var args = new object[parameterTypes.Length];

        for (var index = 0; index < args.Length; index++)
        {
            if (arguments.TryGetValue(parameterTypes[index], out var value))
            {
                args[index] = value;
            }
            else
            {
                args[index] = Resolve(parameterTypes[index]);
            }
        }

        return del.DynamicInvoke(args);
    }
}
