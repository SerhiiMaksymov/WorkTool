namespace WorkTool.Core.Modules.DependencyInjection.Services;

public class DependencyInjector : IDependencyInjector
{
    private readonly DependencyInjectorFields fields;

    public DependencyInjector(
    IReadOnlyDictionary<TypeInformation, InjectorItem>                  injectors,
    IReadOnlyDictionary<AutoInjectMemberIdentifier, InjectorItem>       autoInjects,
    IReadOnlyDictionary<ReservedCtorParameterIdentifier, InjectorItem>  reservedCtorParameters,
    IReadOnlyDictionary<TypeInformation, LazyDependencyInjectorOptions> lazyOptions
    )
    {
        Check(injectors);

        fields = new DependencyInjectorFields(
            injectors,
            autoInjects,
            reservedCtorParameters,
            lazyOptions
        );
    }

    public ReadOnlyMemory<TypeInformation> Inputs  => fields.Inputs;
    public ReadOnlyMemory<TypeInformation> Outputs => fields.Outputs;

    public object Resolve(TypeInformation type)
    {
        if (!fields.Injectors.TryGetValue(type, out var injectorItem))
        {
            throw new TypeNotRegisterException(type.Type);
        }

        BuildExpression(type, injectorItem, out var expression);
        var func  = BuildFunc(expression);
        var value = func.Invoke();

        return value;
    }

    private bool BuildExpression(
    TypeInformation type,
    InjectorItem    injectorItem,
    out Expression  result
    )
    {
        switch (injectorItem.Type)
        {
            case InjectorItemType.Singleton:
            {
                if (fields.CacheSingleton.TryGetValue(type, out result))
                {
                    return true;
                }

                if (UpdateParameters(injectorItem.Expression, out result))
                {
                    var constant = result
                        .ToLambda()
                        .Compile()
                        .DynamicInvoke()
                        .ThrowIfNull()
                        .ToConstant();

                    fields.CacheSingleton.Add(type, constant);

                    return true;
                }

                return false;
            }
            case InjectorItemType.Transient:
            {
                var isFull = UpdateParameters(injectorItem.Expression, out result);

                return isFull;
            }
            default:
            {
                throw new UnreachableException();
            }
        }
    }

    private bool UpdateParameters(Expression expression, out Expression result)
    {
        var isFull = true;

        switch (expression)
        {
            case InvocationExpression invocationExpression:
            {
                var arguments = new List<Expression>();

                foreach (var argument in invocationExpression.Arguments)
                {
                    if (!UpdateParameters(argument, out var value))
                    {
                        isFull = false;
                    }

                    arguments.Add(value);
                }

                result = invocationExpression.Update(invocationExpression.Expression, arguments);

                break;
            }
            case ParameterExpression parameterExpression:
            {
                var parameter = CreateParameter(parameterExpression);

                if (parameter is ParameterExpression)
                {
                    isFull = false;
                }

                result = parameter;

                break;
            }
            case LambdaExpression lambdaExpression:
            {
                var expressions = new List<Expression>();

                foreach (var parameter in lambdaExpression.Parameters)
                {
                    if (!UpdateParameters(parameter, out var value))
                    {
                        isFull = false;
                    }

                    expressions.Add(value);
                }

                result = lambdaExpression.ToInvoke(expressions);

                break;
            }
            case NewExpression newExpression:
            {
                var arguments = new List<Expression>();

                var parameters = newExpression.Constructor is null
                    ? Array.Empty<ParameterInfo>()
                    : newExpression.Constructor.GetParameters();

                var reserveds =
                    new Dictionary<int, (TypeInformation Type, InjectorItem InjectorItem)>();

                for (var index = 0; index < parameters.Length; index++)
                {
                    var identifier = new ReservedCtorParameterIdentifier(
                        newExpression.Type,
                        newExpression.Constructor.ThrowIfNull(),
                        parameters[index]
                    );

                    if (
                        !fields.ReservedCtorParameters.TryGetValue(identifier, out var injectorItem)
                    )
                    {
                        continue;
                    }

                    reserveds.Add(index, (identifier.Parameter.ParameterType, injectorItem));
                }

                for (var index = 0; index < newExpression.Arguments.Count; index++)
                {
                    if (reserveds.TryGetValue(index, out var item))
                    {
                        if (!BuildExpression(item.Type, item.InjectorItem, out var reserved))
                        {
                            isFull = false;
                        }

                        arguments.Add(reserved);

                        continue;
                    }

                    var argument = newExpression.Arguments[index];

                    if (!UpdateParameters(argument, out var value))
                    {
                        isFull = false;
                    }

                    arguments.Add(value);
                }

                result = newExpression.Update(arguments);

                break;
            }
            case MethodCallExpression methodCallExpression:
            {
                var arguments = new List<Expression>();

                foreach (var argument in methodCallExpression.Arguments)
                {
                    if (!UpdateParameters(argument, out var value))
                    {
                        isFull = false;
                    }

                    arguments.Add(value);
                }

                if (methodCallExpression.Object is null)
                {
                    result = methodCallExpression.Update(methodCallExpression.Object, arguments);
                }
                else
                {
                    if (!UpdateParameters(methodCallExpression.Object, out var obj))
                    {
                        isFull = false;
                    }

                    result = methodCallExpression.Update(obj, arguments);
                }

                break;
            }
            case ConstantExpression constantExpression:
            {
                result = constantExpression;

                break;
            }
            default:
            {
                var type = expression.GetType();

                throw new UnreachableException(type.ToString());
            }
        }

        if (!AutoInjectMembers(result, out result))
        {
            isFull = false;
        }

        return isFull;
    }

    private bool AutoInjectMembers(Expression root, out Expression result)
    {
        var isFull            = true;
        var variables         = new List<ParameterExpression>();
        var members           = root.Type.GetMembers();
        var memberExpressions = new List<(MemberInfo Member, Expression Expression)>();

        foreach (var member in members)
        {
            var identifier = new AutoInjectMemberIdentifier(root.Type, member);

            if (!fields.AutoInjectMembers.TryGetValue(identifier, out var injectorItem))
            {
                continue;
            }

            if (
                injectorItem.Expression is LambdaExpression lambdaExpression
             && lambdaExpression.Parameters.IsSingle()
             && lambdaExpression.Body is ParameterExpression parameterExpression
             && lambdaExpression.Parameters[0].Type == parameterExpression.Type
            )
            {
                if (fields.Injectors.ContainsKey(parameterExpression.Type))
                {
                    injectorItem = fields.Injectors[parameterExpression.Type];
                }
                else
                {
                    variables.Add(parameterExpression);
                    memberExpressions.Add((member, parameterExpression));
                    isFull = false;

                    continue;
                }
            }

            if (!BuildExpression(identifier.Member.Type, injectorItem, out var expression))
            {
                variables.Add(GetParameters(expression));
                isFull = false;
            }

            memberExpressions.Add((member, expression));
        }

        if (memberExpressions.IsEmpty())
        {
            result = root;

            return isFull;
        }

        var blockItems   = new List<Expression>();
        var rootVariable = root.Type.ToVariableAutoName();
        blockItems.Add(rootVariable.ToAssign(root));

        foreach (var memberExpression in memberExpressions)
        {
            var assign = rootVariable
                .ToMember(memberExpression.Member)
                .ToAssign(memberExpression.Expression);

            blockItems.Add(assign);
        }

        blockItems.Add(rootVariable);
        result = variables.ToBlock(blockItems);

        return isFull;
    }

    private IEnumerable<ParameterExpression> GetParameters(Expression expression)
    {
        switch (expression)
        {
            case InvocationExpression invocationExpression:
            {
                foreach (var argument in invocationExpression.Arguments)
                {
                    foreach (var parameter in GetParameters(argument))
                    {
                        yield return parameter;
                    }
                }

                break;
            }
            case ParameterExpression parameterExpression:
            {
                yield return parameterExpression;

                break;
            }
            case LambdaExpression lambdaExpression:
            {
                foreach (var parameter in lambdaExpression.Parameters)
                {
                    yield return parameter;
                }

                break;
            }
            case NewExpression newExpression:
            {
                foreach (var argument in newExpression.Arguments)
                {
                    foreach (var parameter in GetParameters(argument))
                    {
                        yield return parameter;
                    }
                }

                break;
            }
            case MethodCallExpression methodCallExpression:
            {
                foreach (var argument in methodCallExpression.Arguments)
                {
                    foreach (var parameter in GetParameters(argument))
                    {
                        yield return parameter;
                    }
                }

                if (methodCallExpression.Object is not null)
                {
                    foreach (var parameter in GetParameters(methodCallExpression.Object))
                    {
                        yield return parameter;
                    }
                }

                break;
            }
            case ConstantExpression:
            {
                break;
            }
            default:
            {
                var type = expression.GetType();

                throw new UnreachableException(type.ToString());
            }
        }
    }

    private Expression CreateParameter(ParameterExpression parameterExpression)
    {
        if (!fields.Injectors.TryGetValue(parameterExpression.Type, out var injectorItem))
        {
            return parameterExpression;
        }

        BuildExpression(parameterExpression.Type, injectorItem, out var result);

        return result;
    }

    private Func<object> BuildFunc(Expression expression)
    {
        var result = expression.ToLambda().Compile().ThrowIfIsNot<Func<object>>();

        return result;
    }

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
            var keyType          = injector.Key.Type;
            var injectorType     = injector.Value.Expression.Type;
            var isAssignableFrom = injector.Value.Expression.Type.IsAssignableFrom(keyType);

            if (keyType != injectorType && isAssignableFrom)
            {
                throw new NotCovertException(injector.Key.Type, injector.Value.Expression.Type);
            }

            var types = GetParameters(injector.Value.Expression)
                .Select(x => (TypeInformation)x.Type);

            listParameterTypes.AddRange(types);

            if (listParameterTypes.Contains(injector.Key.Type))
            {
                throw new RecursionTypeExpressionInvokeException(
                    injector.Key.Type,
                    injector.Value.Expression
                );
            }

            listParameterTypes.Clear();
        }
    }

#endregion

    public object? Invoke(Delegate del, DictionarySpan<TypeInformation, object> arguments)
    {
        var parameterTypes = del.GetParameterTypes();
        var args           = new object[parameterTypes.Length];

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
        if (IsLazy(type))
        {
            return GetLazyStatus(type);
        }

        if (!fields.Injectors.TryGetValue(type, out var injectorItem))
        {
            throw new TypeNotRegisterException(type.Type);
        }

        BuildExpression(type, injectorItem, out var expression);

        return new DependencyStatus(type, expression);
    }

    private DependencyStatus GetLazyStatus(TypeInformation type)
    {
        var instanceType = type.GenericTypeArguments.GetSingle();

        if (!fields.Injectors.TryGetValue(instanceType, out var injectorItem))
        {
            throw new TypeNotRegisterException(instanceType.Type);
        }

        BuildExpression(type, injectorItem, out var expression);
        var options = fields.LazyOptions.Get(instanceType, LazyDependencyInjectorOptions.None);

        switch (options)
        {
            case LazyDependencyInjectorOptions.None:
            {
                var constructor = type.Type.GetConstructor(
                    new[]
                    {
                        typeof(Func<>).MakeGenericType(instanceType.Type),
                        typeof(LazyThreadSafetyMode)
                    }).ThrowIfNull();

                return new DependencyStatus(
                    type,
                    constructor.ToNew(expression.ToLambda(), LazyThreadSafetyMode.None.ToConstant()));
            }
            case LazyDependencyInjectorOptions.PublicationOnly:
            {
                var constructor = type.Type.GetConstructor(
                    new[]
                    {
                        typeof(Func<>).MakeGenericType(instanceType.Type),
                        typeof(LazyThreadSafetyMode)
                    }).ThrowIfNull();

                return new DependencyStatus(
                    type,
                    constructor.ToNew(expression.ToLambda(), LazyThreadSafetyMode.PublicationOnly.ToConstant()));
            }
            case LazyDependencyInjectorOptions.ExecutionAndPublication:
            {
                var constructor = type.Type.GetConstructor(
                    new[]
                    {
                        typeof(Func<>).MakeGenericType(instanceType.Type),
                        typeof(LazyThreadSafetyMode)
                    }).ThrowIfNull();

                return new DependencyStatus(
                    type,
                    constructor.ToNew(
                        expression.ToLambda(),
                        LazyThreadSafetyMode.ExecutionAndPublication.ToConstant()));
            }
            case LazyDependencyInjectorOptions.ThreadSafe:
            {
                var constructor = type.Type.GetConstructor(
                    new[]
                    {
                        typeof(Func<>).MakeGenericType(instanceType.Type),
                        typeof(bool)
                    }).ThrowIfNull();

                return new DependencyStatus(type, constructor.ToNew(expression.ToLambda(), true.ToConstant()));
            }
            default: throw new UnreachableException();
        }
    }

    private bool IsLazy(TypeInformation type)
    {
        if (!type.IsGenericType)
        {
            return false;
        }

        if (type.Type.GetGenericTypeDefinition() == typeof(Lazy<>))
        {
            return true;
        }

        return false;
    }
}