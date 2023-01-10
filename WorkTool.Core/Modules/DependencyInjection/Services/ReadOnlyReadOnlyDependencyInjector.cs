namespace WorkTool.Core.Modules.DependencyInjection.Services;

public class ReadOnlyReadOnlyDependencyInjector : IReadOnlyDependencyInjector
{
    private readonly Dictionary<ReserveIdentifier, InjectorItem> reserves;
    private readonly Dictionary<ReserveIdentifier, Expression> cacheReservesExpressions;
    private readonly Dictionary<AutoInjectIdentifier, InjectorItem> autoInjects;
    private readonly Dictionary<Type, InjectorItem> injectors;
    private readonly Dictionary<Type, object> cacheSingletonValues;
    private readonly Dictionary<Type, Func<object>> cacheTransientValues;
    private readonly Dictionary<Type, Expression> cacheExpressions;
    private readonly IRandom<string> randomString;

    public ReadOnlyReadOnlyDependencyInjector(
        IReadOnlyDictionary<Type, InjectorItem> injectors,
        IReadOnlyDictionary<ReserveIdentifier, InjectorItem> reserves,
        IReadOnlyDictionary<AutoInjectIdentifier, InjectorItem> autoInjects,
        IRandom<string> randomString
    )
    {
        this.randomString = randomString;
        this.autoInjects = new(autoInjects);
        this.injectors = new(injectors);
        this.reserves = new(reserves);
        cacheSingletonValues = new() { { typeof(IResolver), this }, { typeof(IInvoker), this } };
        cacheTransientValues = new();
        cacheReservesExpressions = new();

        cacheExpressions = new()
        {
            { typeof(IResolver), this.ToConstant() },
            { typeof(IInvoker), this.ToConstant() }
        };
    }

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
        Type type,
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

    private void CacheValue(Type type)
    {
        if (injectors.TryGetValue(type, out var injectorItem))
        {
            switch (injectorItem.Type)
            {
                case InjectorItemType.Singleton:
                {
                    CacheSingletonValue(type);

                    return;
                }
                case InjectorItemType.Transient:
                {
                    CacheTransientValue(type);

                    return;
                }
                default:
                {
                    throw new UnreachableException();
                }
            }
        }

        CacheTransientValue(type);
    }

    private void CacheTransientValue(Type type)
    {
        if (!injectors.ContainsKey(type))
        {
            CacheTransientDefaultValue(type);

            return;
        }

        var injector = injectors[type];
        var parameters = injector.Delegate.Method.GetParameters();

        if (parameters.Length == 1 && parameters[0].ParameterType == type)
        {
            CacheTransientDefaultValue(type);

            return;
        }

        var expressions = ConstructorParametersToExpressions(type, parameters);
        var instance = injector.Delegate.Target.ThrowIfNull().ToConstant();
        var newExpression = injector.Delegate.Method.ToCall(instance, expressions);
        var result = GetAutoInjectExpression(type, newExpression);

        var func = result.Convert(typeof(object)).Lambda().Compile().ThrowIfIsNot<Func<object>>();

        cacheExpressions.Add(type, result);
        cacheTransientValues.Add(type, func);
    }

    private void CacheTransientDefaultValue(Type type)
    {
        var constructor = GetSingleConstructor(type);

        if (constructor is null)
        {
            CacheTransientValueTypeValue(type);

            return;
        }

        var parameters = constructor.GetParameters();
        var expressions = ConstructorParametersToExpressions(type, parameters);
        var newExpression = constructor.ToNew(expressions);
        var result = GetAutoInjectExpression(type, newExpression);

        var obj = result.Convert(typeof(object)).Lambda().Compile().ThrowIfIsNot<Func<object>>();

        cacheExpressions.Add(type, result);
        cacheTransientValues.Add(type, obj);
    }

    private void CacheTransientValueTypeValue(Type type)
    {
        if (!type.IsValueType)
        {
            throw new NotHaveConstructorException(type);
        }

        var obj = type.ToNew().ThrowIfNull();
        var result = GetAutoInjectExpression(type, obj);
        cacheExpressions.Add(type, result);

        var transientValue = result
            .Convert(typeof(object))
            .Lambda()
            .Compile()
            .ThrowIfIsNot<Func<object>>();

        cacheTransientValues.Add(type, transientValue);
    }

    private void CacheSingletonValue(Type type)
    {
        if (!injectors.ContainsKey(type))
        {
            CacheSingletonDefaultValue(type);

            return;
        }

        var injector = injectors[type];
        var parameters = injector.Delegate.Method.GetParameters();

        if (parameters.Length == 1 && parameters[0].ParameterType == type)
        {
            CacheSingletonDefaultValue(type);

            return;
        }

        var expressions = ConstructorParametersToExpressions(type, parameters);

        var newExpression = injector.Delegate.Method
            .ToCall(injector.Delegate.Target.ThrowIfNull().ToConstant(), expressions)
            .Lambda()
            .Compile();

        var obj = newExpression.DynamicInvoke().ThrowIfNull();
        var result = GetAutoInjectExpression(type, obj.ToConstant());
        var resultObj = result.Lambda().Compile().DynamicInvoke().ThrowIfNull();
        cacheExpressions.Add(type, result);
        cacheSingletonValues.Add(type, resultObj);
    }

    private void CacheSingletonValueTypeValue(Type type)
    {
        if (!type.IsValueType)
        {
            throw new NotHaveConstructorException(type);
        }

        var obj = Activator.CreateInstance(type).ThrowIfNull();
        var result = GetAutoInjectExpression(type, obj.ToConstant());
        cacheExpressions.Add(type, result);
        cacheSingletonValues.Add(type, result.Lambda().Compile().DynamicInvoke().ThrowIfNull());
    }

    private void CacheSingletonDefaultValue(Type type)
    {
        var constructor = GetSingleConstructor(type);

        if (constructor is null)
        {
            CacheSingletonValueTypeValue(type);

            return;
        }

        var parameters = constructor.GetParameters();
        var expressions = ConstructorParametersToExpressions(type, parameters);
        var newExpression = constructor.ToNew(expressions).Lambda().Compile();
        var obj = newExpression.DynamicInvoke().ThrowIfNull();
        var result = GetAutoInjectExpression(type, obj.ToConstant());
        cacheExpressions.Add(type, result);
        cacheSingletonValues.Add(type, result.Lambda().Compile().DynamicInvoke().ThrowIfNull());
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
        var parameters = del.Method.GetParameters();
        var args = new object[parameters.Length];

        for (var index = 0; index < args.Length; index++)
        {
            if (arguments.TryGetValue(parameters[index].ParameterType, out var value))
            {
                args[index] = value;
            }
            else
            {
                args[index] = Resolve(parameters[index].ParameterType);
            }
        }

        return del.DynamicInvoke(args);
    }
}
