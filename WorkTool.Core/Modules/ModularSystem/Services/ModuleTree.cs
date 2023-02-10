namespace WorkTool.Core.Modules.ModularSystem.Services;

public class ModuleTree : IModule, IResolver, IInvoker
{
    private readonly Tree<Guid, IModule> tree;
    private readonly Dictionary<TypeInformation, Func<object>> cache;
    private readonly Expression thisExpression;

    public ModuleTree(Tree<Guid, IModule> tree)
    {
        this.tree = tree;
        cache = new();
        Id = Guid.NewGuid();
        var inputs = new List<TypeInformation>();
        var outputs = new List<TypeInformation>();
        var ends = this.tree.GetEnds();

        foreach (var node in ends)
        {
            AddTypes(inputs, outputs, node);
        }

        var outputsArray = outputs
            .Distinct()
            .Concat(new TypeInformation[] { typeof(IResolver), typeof(IInvoker) })
            .OrderBy(x => x.ToString())
            .ToArray();

        Outputs = outputsArray;
        Inputs = inputs.Distinct().Where(x => !outputsArray.Contains(x)).ToArray();
        thisExpression = this.ToConstant();
    }

    public Guid Id { get; }
    public ReadOnlyMemory<TypeInformation> Inputs { get; }
    public ReadOnlyMemory<TypeInformation> Outputs { get; }
    public bool IsFull => Inputs.IsEmpty;

    public DependencyStatus GetStatus(TypeInformation type)
    {
        if (typeof(IResolver) == type)
        {
            return new DependencyStatus(type, thisExpression);
        }

        if (typeof(IInvoker) == type)
        {
            return new DependencyStatus(type, thisExpression);
        }

        if (!IsTypeContains(type))
        {
            throw new TypeNotRegisterException(type.Type);
        }

        if (IsTypeContains(tree.Root.Value.Outputs, type))
        {
            var rootStatus = tree.Root.Value.GetStatus(type);

            return rootStatus;
        }

        var status = GetDependencyStatus(tree.Root, type).ThrowIfNullStruct();

        return status;
    }

    public object GetObject(TypeInformation type)
    {
        if (typeof(IResolver) == type)
        {
            return this;
        }

        if (typeof(IInvoker) == type)
        {
            return this;
        }

        if (!IsTypeContains(type))
        {
            throw new TypeNotRegisterException(type.Type);
        }

        if (cache.TryGetValue(type, out var func))
        {
            var value = func.Invoke();

            return value;
        }

        var status = GetStatus(type);
        var expression = UpdateExpression(status.Expression);

        if (expression.Type.IsValueType)
        {
            expression = expression.ToConvert(typeof(object));
        }

        var lambda = expression.ToLambda();
        func = lambda.Compile().ThrowIfIsNot<Func<object>>();
        var result = func.Invoke();
        cache.Add(type, func);

        return result;
    }

    public object Resolve(TypeInformation type)
    {
        return GetObject(type);
    }

    private bool IsTypeContains(ReadOnlyMemory<TypeInformation> outputs, TypeInformation type)
    {
        if (!outputs.Span.Contains(type))
        {
            if (!type.Type.IsGenericType)
            {
                return false;
            }
            
            if (type.Type.GetGenericTypeDefinition() != typeof(Lazy<>))
            {
                return false;
            }

            var argument = type.Type.GenericTypeArguments.Single();

            if (!outputs.Span.Contains(argument))
            {
                return false;
            }
        }

        return true;
    }

    private bool IsTypeContains(TypeInformation type)
    {
        return IsTypeContains(Outputs, type);
    }
    
    private bool IsTypeContains(TreeNode<Guid, IModule> node, TypeInformation type)
    {
        return IsTypeContains(node.Value.Outputs, type);
    }

    private Expression UpdateExpression(Expression expression)
    {
        switch (expression)
        {
            case InvocationExpression invocationExpression:
            {
                var arguments = new List<Expression>();

                foreach (var argument in invocationExpression.Arguments)
                {
                    arguments.Add(UpdateExpression(argument));
                }

                var result = invocationExpression.Update(
                    invocationExpression.Expression,
                    arguments
                );

                return result;
            }
            case ParameterExpression parameterExpression:
            {
                var result = CreateParameter(parameterExpression);

                return result;
            }
            case NewExpression newExpression:
            {
                var arguments = new List<Expression>();

                foreach (var argument in newExpression.Arguments)
                {
                    var argumentExpression = UpdateExpression(argument);

                    if (argumentExpression.Type.IsValueType)
                    {
                        argumentExpression = argumentExpression.ToConvert(argument.Type);
                    }

                    arguments.Add(argumentExpression);
                }

                return newExpression.Update(arguments);
            }
            case BlockExpression blockExpression:
            {
                var expressions = new List<Expression>();
                var blockExpressionItems = blockExpression.Expressions.Take(
                    blockExpression.Expressions.Count - 1
                );
                var blockResult = blockExpression.Expressions
                    .Last()
                    .ThrowIfIsNot<ParameterExpression>();

                foreach (var blockExpressionItem in blockExpressionItems)
                {
                    expressions.Add(UpdateExpression(blockExpressionItem));
                }

                expressions.Add(blockResult);
                var result = blockExpression.Update(blockResult.AsArray(), expressions);

                return result;
            }
            case ConstantExpression constantExpression:
            {
                return constantExpression;
            }
            case BinaryExpression binaryExpression:
            {
                var conversion = binaryExpression.Conversion is null
                    ? null
                    : UpdateExpression(binaryExpression).ThrowIfIsNot<LambdaExpression>();
                var right = UpdateExpression(binaryExpression.Right);
                var result = binaryExpression.Update(binaryExpression.Left, conversion, right);

                return result;
            }
            case MethodCallExpression methodCallExpression:
            {
                var obj = methodCallExpression.Object is null
                    ? null
                    : UpdateExpression(methodCallExpression.Object);
                var arguments = new List<Expression>();

                foreach (var argument in methodCallExpression.Arguments)
                {
                    arguments.Add(UpdateExpression(argument));
                }

                return methodCallExpression.Update(obj, arguments);
            }
            case LambdaExpression lambdaExpression:
            {
                var body = UpdateExpression(lambdaExpression.Body);
                var expressions = new List<Expression>();

                foreach (var parameter in lambdaExpression.Parameters)
                {
                    expressions.Add(UpdateExpression(parameter));
                }

                if (expressions.Any())
                {
                    var result = body.ToLambda().ToInvoke(expressions).ToLambda();

                    return result;
                }
                else
                {
                    var result = body.ToLambda();

                    return result;
                }
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
        var status = GetStatus(parameterExpression.Type);
        var expression = UpdateExpression(status.Expression);

        return expression;
    }

    private DependencyStatus? GetDependencyStatus(
        TreeNode<Guid, IModule> node,
        TypeInformation type
    )
    {
        if (typeof(IResolver) == type)
        {
            return new DependencyStatus(type, thisExpression);
        }

        if (typeof(IInvoker) == type)
        {
            return new DependencyStatus(type, thisExpression);
        }

        foreach (var treeNode in node.Nodes)
        {
            if (IsTypeContains(treeNode, type))
            {
                var status = treeNode.Value.GetStatus(type);

                return status;
            }
        }

        foreach (var treeNode in node.Nodes)
        {
            var status = GetDependencyStatus(treeNode, type);

            if (status is null)
            {
                continue;
            }

            return status;
        }

        throw new TypeNotRegisterException(type.Type);
    }

    private void AddTypes(
        List<TypeInformation> inputs,
        List<TypeInformation> outputs,
        TreeNode<Guid, IModule> node
    )
    {
        outputs.AddRange(node.Value.Outputs.ToArray());
        inputs.AddRange(node.Value.Inputs.ToArray());

        if (node.Parent is null)
        {
            return;
        }

        AddTypes(inputs, outputs, node.Parent);
    }

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
}
