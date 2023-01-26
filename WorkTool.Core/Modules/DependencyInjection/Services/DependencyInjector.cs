namespace WorkTool.Core.Modules.DependencyInjection.Services;

public class DependencyInjector : IDependencyInjector
{
    private readonly DependencyInjectorFields fields;

    public DependencyInjector(
        IReadOnlyDictionary<TypeInformation, InjectorItem> injectors,
        IReadOnlyDictionary<AutoInjectIdentifier, InjectorItem> autoInjects,
        IReadOnlyDictionary<TypeInformation, IEnumerable<InjectorItem>> collections,
        IRandom<string> randomString,
        InjectorItem resolver,
        InjectorItem invoker
    )
    {
        Check(injectors);

        fields = new DependencyInjectorFields(
            injectors,
            autoInjects,
            collections,
            randomString,
            resolver,
            invoker
        );
    }

    public DependencyInjector(
        IReadOnlyDictionary<TypeInformation, InjectorItem> injectors,
        IReadOnlyDictionary<AutoInjectIdentifier, InjectorItem> autoInjects,
        IReadOnlyDictionary<TypeInformation, IEnumerable<InjectorItem>> collections,
        IRandom<string> randomString
    )
    {
        Check(injectors);
        var item = new InjectorItem(InjectorItemType.Singleton, () => this);

        fields = new DependencyInjectorFields(
            injectors,
            autoInjects,
            collections,
            randomString,
            item,
            item
        );
    }

    public ReadOnlyMemory<TypeInformation> Inputs => fields.Inputs;
    public ReadOnlyMemory<TypeInformation> Outputs => fields.Outputs;

    public object Resolve(TypeInformation type)
    {
        var cache = GetCacheValue(type);

        if (cache is not null)
        {
            return cache;
        }

        CacheValue(type);
        var result = GetCacheValue(type);

        if (result is null)
        {
            throw new TypeNotRegisterException(type.Type);
        }

        return result;
    }

#region AutoInject

    private Expression GetAutoInjectExpression(TypeInformation type, Expression root)
    {
        var expressions = GetTypeMembersExpressions(type);

        if (expressions.IsEmpty())
        {
            return root;
        }

        var id = fields.RandomString.GetRandom();
        var variable = root.Type.ToVariable(CreateVariableName());

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

    private List<(MemberInfo Member, Expression Expression)> GetTypeMembersExpressions(
        TypeInformation type
    )
    {
        var list = new List<(MemberInfo Member, Expression Expression)>();

        foreach (var member in GetAutoInjectMembers(type))
        {
            list.Add((member, GetAutoInjectsExpression(type, member)));
        }

        return list;
    }

    private Expression GetAutoInjectsExpression(TypeInformation type, MemberInfo member)
    {
        var autoInjectIdentifier = new AutoInjectIdentifier(type, member);
        var injectorItem = fields.AutoInjects[autoInjectIdentifier];

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
            TryGetOrCacheExpression(del.Method.ReturnType, out var result);

            return result.ThrowIfNull();
        }

        var expressions = DelegateParametersToExpressions(parameters);

        var constant = del.ToCall(expressions)
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
            TryGetOrCacheExpression(del.Method.ReturnType, out var result);

            return result.ThrowIfNull();
        }

        var expressions = DelegateParametersToExpressions(parameters);
        var call = del.ToCall(expressions);

        return call;
    }

    private List<MemberInfo> GetAutoInjectMembers(TypeInformation type)
    {
        var result = new List<MemberInfo>();

        var members = type.Members.Span
            .ToArray()
            .Where(x => x is PropertyInfo { CanWrite: true } or FieldInfo { IsInitOnly: false });

        foreach (var member in members)
        {
            var autoInjectIdentifier = new AutoInjectIdentifier(type, member);

            if (!fields.AutoInjects.ContainsKey(autoInjectIdentifier))
            {
                continue;
            }

            result.Add(member);
        }

        return result;
    }

#endregion

#region Injector

    private ExpressionScope DelegateParametersToExpressions(IEnumerable<ParameterInfo> parameters)
    {
        var expressions = new List<Expression>();
        var variables = new List<ParameterExpression>();

        foreach (var parameter in parameters)
        {
            if (parameter.ParameterType.IsClosure())
            {
                continue;
            }

            if (TryGetOrCacheExpression(parameter.ParameterType, out var expression))
            {
                expression = expression.ThrowIfNull();

                if (expression.Type.IsValueType && !parameter.ParameterType.IsValueType)
                {
                    expression = expression.Convert(parameter.ParameterType);
                }

                expressions.Add(expression);
            }
            else
            {
                var expressionVariable = parameter.ParameterType.ToVariable(CreateVariableName());
                expressions.Add(expressionVariable);
                variables.Add(expressionVariable);
            }
        }

        return new ExpressionScope(expressions.ToArray(), variables.ToArray());
    }

    private bool TryGetOrCacheExpression(
        Type type,
        [MaybeNullWhen(false)] out Expression expression
    )
    {
        expression = GetCacheExpression(type);

        if (expression is null)
        {
            CacheValue(type);
            expression = GetCacheExpression(type);
        }

        if (expression is null)
        {
            return false;
        }

        return true;
    }

    private Expression? GetCacheExpression(TypeInformation type)
    {
        if (fields.CacheExpressions.TryGetValue(type, out var expression))
        {
            return expression;
        }

        return null;
    }

    private object? GetCacheValue(TypeInformation type)
    {
        if (fields.CacheSingletonValues.TryGetValue(type, out var singletonValue))
        {
            return singletonValue;
        }

        if (fields.CacheTransientValues.TryGetValue(type, out var transientValue))
        {
            var value = transientValue.Invoke();

            return value;
        }

        return null;
    }

    private void CacheCollection(TypeInformation typeEnumerable)
    {
        var type = typeEnumerable.GenericTypeArguments.Span[0];

        if (fields.Collections.TryGetValue(type, out var list))
        {
            CreateCollectionExpression(typeEnumerable, type, list);

            return;
        }

        throw new TypeNotRegisterException(typeEnumerable.Type);
    }

    private ExpressionScope CreateExpressions(Type type, IEnumerable<InjectorItem> items)
    {
        var variables = new List<ParameterExpression>();
        var expressions = new List<Expression>();

        foreach (var item in items)
        {
            switch (item.Type)
            {
                case InjectorItemType.Singleton:
                {
                    var scope = CreateSingletonExpression(type, item);
                    variables.Add(scope.Variables.ToArray());
                    expressions.Add(scope.Expressions.ToArray());

                    break;
                }
                case InjectorItemType.Transient:
                {
                    var scope = CreateTransientExpression(type, item);
                    variables.Add(scope.Variables.ToArray());
                    expressions.Add(scope.Expressions.ToArray());

                    break;
                }
                default:
                {
                    throw new UnreachableException();
                }
            }
        }

        return new ExpressionScope(expressions.ToArray(), variables.ToArray());
    }

    private ExpressionScope CreateCollectionExpression(
        TypeInformation typeEnumerable,
        TypeInformation type,
        IEnumerable<InjectorItem> items
    )
    {
        var listType = typeof(List<>).MakeGenericType(type.Type);
        var listAddMethod = listType.GetMethod(nameof(List<object>.Add)).ThrowIfNull();
        var listAddRangeMethod = listType.GetMethod(nameof(List<object>.AddRange)).ThrowIfNull();
        var listExpressions = CreateExpressions(type.Type, items);
        var variable = listType.ToVariable(CreateVariableName());
        var blockItems = new List<Expression> { variable.Assign(listType.ToNew()) };

        foreach (var item in listExpressions.Expressions.ToArray())
        {
            if (typeEnumerable.Type.IsAssignableFrom(item.Type))
            {
                blockItems.Add(
                    listAddRangeMethod.ToCall(variable, item.Convert(typeEnumerable.Type))
                );
            }
            else
            {
                blockItems.Add(listAddMethod.ToCall(variable, item.Convert(type.Type)));
            }
        }

        blockItems.Add(variable);
        var expression = Expression.Block(new[] { variable }, blockItems);
        var scope = new ExpressionScope(expression.AsArray(), listExpressions.Variables);
        CacheTransient(typeEnumerable, scope);

        return scope;
    }

    private string CreateVariableName()
    {
        var id = fields.RandomString.GetRandom();

        return $"newInstance{id}";
    }

    private void CacheValue(TypeInformation type)
    {
        if (type.IsEnumerable())
        {
            CacheCollection(type);

            return;
        }

        if (fields.Injectors.TryGetValue(type, out var injectorItem))
        {
            switch (injectorItem.Type)
            {
                case InjectorItemType.Singleton:
                {
                    var expression = CreateSingletonExpression(type);
                    CacheSingleton(type, expression);

                    return;
                }
                case InjectorItemType.Transient:
                {
                    var scope = CreateTransientExpression(type);
                    CacheTransient(type, scope);

                    return;
                }
                default:
                {
                    throw new UnreachableException();
                }
            }
        }
    }

    private ExpressionScope CreateTransientExpression(TypeInformation type)
    {
        if (!fields.Injectors.ContainsKey(type))
        {
            return CreateTransientDefaultValue(type);
        }

        var injector = fields.Injectors[type];

        return CreateTransientExpression(type, injector);
    }

    private ExpressionScope CreateTransientExpression(TypeInformation type, InjectorItem injector)
    {
        var parameters = injector.Delegate.Method.GetParameters();

        if (parameters.Length == 1 && parameters[0].ParameterType == type)
        {
            return CreateTransientDefaultValue(type);
        }

        var expressions = DelegateParametersToExpressions(parameters);
        var newExpression = injector.Delegate.ToCall(expressions);
        var result = GetAutoInjectExpression(type, newExpression);

        return new ExpressionScope(result.AsArray(), expressions.Variables);
    }

    private ExpressionScope CreateTransientDefaultValue(TypeInformation type)
    {
        var constructor = type.GetSingleConstructor();

        if (constructor is null)
        {
            return CreateTransientValueTypeValue(type);
        }

        var parameters = constructor.GetParameters();
        var expressions = DelegateParametersToExpressions(parameters);
        var newExpression = constructor.ToNew(expressions);
        var result = GetAutoInjectExpression(type, newExpression);

        return new ExpressionScope(result.AsArray(), Array.Empty<ParameterExpression>());
    }

    private ExpressionScope CreateTransientValueTypeValue(TypeInformation type)
    {
        if (!type.IsValueType)
        {
            throw new NotHaveConstructorException(type.Type);
        }

        var obj = type.Type.ToNew().ThrowIfNull();
        var result = GetAutoInjectExpression(type, obj);

        return new ExpressionScope(result.AsArray(), Array.Empty<ParameterExpression>());
    }

    private void CacheTransient(TypeInformation type, ExpressionScope scope)
    {
        if (scope.Expressions.Length > 1)
        {
            return;
        }
        
        var expression = scope.Expressions.Span[0];

        if (!scope.Variables.IsEmpty)
        {
            return;
        }
        
        var transientValue = expression
            .Convert(typeof(object))
            .Lambda()
            .Compile()
            .ThrowIfIsNot<Func<object>>();

        fields.CacheExpressions.Add(type, expression);
        fields.CacheTransientValues.Add(type, transientValue);
    }

    private ExpressionScope CreateSingletonExpression(TypeInformation type)
    {
        if (!fields.Injectors.ContainsKey(type))
        {
            return CreateSingletonDefaultValue(type);
        }

        var injector = fields.Injectors[type];

        return CreateSingletonExpression(type, injector);
    }

    private ExpressionScope CreateSingletonExpression(TypeInformation type, InjectorItem injector)
    {
        var parameters = injector.Delegate.Method.GetParameters();

        if (parameters.Length == 1 && parameters[0].ParameterType == type)
        {
            return CreateSingletonDefaultValue(type);
        }

        var expressions = DelegateParametersToExpressions(parameters);
        var newExpression = injector.Delegate.ToCall(expressions).Lambda().Compile();
        var obj = newExpression.DynamicInvoke().ThrowIfNull();
        var result = GetAutoInjectExpression(type, obj.ToConstant());

        return new ExpressionScope(result.AsArray(), Array.Empty<ParameterExpression>());
    }

    private ExpressionScope CreateSingletonValueTypeValue(TypeInformation type)
    {
        if (!type.IsValueType)
        {
            throw new NotHaveConstructorException(type.Type);
        }

        var obj = Activator.CreateInstance(type.Type).ThrowIfNull();
        var result = GetAutoInjectExpression(type.Type, obj.ToConstant());

        return new ExpressionScope(result.AsArray(), Array.Empty<ParameterExpression>());
    }

    private void CacheSingleton(TypeInformation type, ExpressionScope scope)
    {
        if (scope.Expressions.Length > 1)
        {
            return;
        }

        if (!scope.Variables.IsEmpty)
        {
            return;
        }

        var expression = scope.Expressions.Span[0];
        var value = expression.Lambda().Compile().DynamicInvoke().ThrowIfNull();
        fields.CacheExpressions.Add(type, value.ToConstant());
        fields.CacheSingletonValues.Add(type, value);
    }

    private ExpressionScope CreateSingletonDefaultValue(TypeInformation type)
    {
        var constructor = type.GetSingleConstructor();

        if (constructor is null)
        {
            return CreateSingletonValueTypeValue(type);
        }

        var parameters = constructor.GetParameters();
        var expressions = DelegateParametersToExpressions(parameters);
        var newExpression = constructor.ToNew(expressions).Lambda().Compile();
        var obj = newExpression.DynamicInvoke().ThrowIfNull();
        var result = GetAutoInjectExpression(type.Type, obj.ToConstant());

        return new ExpressionScope(result.AsArray(), Array.Empty<ParameterExpression>());
    }

#endregion

#region Checks

    private void Check(IReadOnlyDictionary<TypeInformation, InjectorItem> injectors)
    {
        CheckInjectors(injectors);
    }

    private void CheckInjectors(IReadOnlyDictionary<TypeInformation, InjectorItem> injectors)
    {
        var listParameterTypes = new List<TypeInformation>();

        foreach (var injector in injectors)
        {
            if (
                injector.Key.Type != injector.Value.Delegate.Method.ReturnType
                && injector.Value.Delegate.Method.ReturnType.IsAssignableFrom(injector.Key.Type)
            )
            {
                throw new NotCovertException(
                    injector.Key.Type,
                    injector.Value.Delegate.Method.ReturnType
                );
            }

            var parameters = injector.Value.Delegate.Method.GetParameters().ToArray();

            foreach (var parameter in parameters)
            {
                listParameterTypes.Add(parameter.ParameterType);
            }

            if (listParameterTypes.Contains(injector.Key.Type))
            {
                throw new RecursionTypeInvokeException(injector.Key.Type, injector.Value.Delegate);
            }

            listParameterTypes.Clear();
        }
    }

#endregion

    public object? Invoke(Delegate del, DictionarySpan<TypeInformation, object> arguments)
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

    public DependencyStatus GetStatus(TypeInformation type)
    {
        if (type.IsEnumerable())
        {
            return GetEnumerableStatus(type);
        }

        if (!fields.Injectors.TryGetValue(type, out var item))
        {
            throw new TypeNotRegisterException(type.Type);
        }

        return GetSingleStatus(type, item.Delegate);
    }

    private DependencyStatus GetSingleStatus(TypeInformation type, Delegate del)
    {
        var parameters = del.Method.GetParameters();
        var neededParameters = new List<ParameterInfo>();
        var parameterValues = new Dictionary<ParameterInfo, Expression>();

        foreach (var parameter in parameters)
        {
            if (parameter.ParameterType.IsClosure())
            {
                continue;
            }

            if (Outputs.Span.Contains(parameter.ParameterType))
            {
                var expression = GetCacheExpression(parameter.ParameterType);

                if (expression is null)
                {
                    CacheValue(parameter.ParameterType);
                    expression = GetCacheExpression(parameter.ParameterType).ThrowIfNull();
                }

                parameterValues.Add(parameter, expression);
            }

            neededParameters.Add(parameter);
        }

        return new DependencyStatus(type, neededParameters.ToArray(), del, parameterValues);
    }

    private DependencyStatus GetEnumerableStatus(TypeInformation typeEnumerable)
    {
        var itemType = typeEnumerable.GenericTypeArguments.Span[0];

        if (!fields.Collections.TryGetValue(itemType, out var items))
        {
            throw new TypeNotRegisterException(typeEnumerable.Type);
        }

        var scope = CreateCollectionExpression(typeEnumerable, itemType, items);

        if (scope.Expressions.Length > 1)
        {
            throw new UnreachableException();
        }

        var expression = scope.Expressions.Span[0];

        if (scope.Variables.IsEmpty)
        {
            var del = expression.Lambda().Compile();

            return GetSingleStatus(typeEnumerable, del);
        }
        else
        {
            var del = Expression
                .Block(scope.Variables.ToArray(), scope.Expressions.ToArray())
                .Lambda()
                .Compile();

            return GetSingleStatus(typeEnumerable, del);
        }
    }
}
