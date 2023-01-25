using WorkTool.Core.Modules.Graph.Extensions;

namespace WorkTool.Core.Modules.ModularSystem.Services;

public class ModuleTree : IModule, IResolver
{
    private readonly Tree<Guid, IModule> tree;
    private readonly Dictionary<TypeInformation, Func<object>> cache;

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
            .Concat(new TypeInformation[] { typeof(IResolver) })
            .ToArray();
        
        Outputs = outputsArray;
        Inputs = inputs.Distinct().Where(x => !outputsArray.Contains(x)).ToArray();
    }

    public Guid Id { get; }
    public ReadOnlyMemory<TypeInformation> Inputs { get; }
    public ReadOnlyMemory<TypeInformation> Outputs { get; }
    public bool IsFull => Inputs.IsEmpty;

    public DependencyStatus GetStatus(TypeInformation type)
    {
        if (!Outputs.Span.Contains(type))
        {
            throw new TypeNotRegisterException(type.Type);
        }

        if (tree.Root.Value.Outputs.Span.Contains(type))
        {
            return tree.Root.Value.GetStatus(type);
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

        if (!Outputs.Span.Contains(type))
        {
            throw new TypeNotRegisterException(type.Type);
        }

        if (cache.TryGetValue(type, out var func))
        {
            var value = func.Invoke();

            return value;
        }

        var status = GetStatus(type);
        var expression = CreateExpression(status);
        func = expression.Lambda().Compile().ThrowIfIsNot<Func<object>>();
        var result = func.Invoke();

        return result;
    }

    public object Resolve(TypeInformation type)
    {
        return GetObject(type);
    }

    private Expression CreateExpression(DependencyStatus status)
    {
        var parameters = new Dictionary<ParameterInfo, Expression>();

        foreach (var neededParameter in status.NeededParameters.Span)
        {
            var statusParameter = GetStatus(neededParameter.ParameterType);
            var expression = CreateExpression(statusParameter);
            parameters.Add(neededParameter, expression);
        }

        var expressionCache = status.CreateExpression(parameters);
        var func = expressionCache.Lambda().Compile().ThrowIfIsNot<Func<object>>();
        cache.Add(status.Type, func);

        return status.CreateExpression(parameters);
    }

    private DependencyStatus? GetDependencyStatus(
        TreeNode<Guid, IModule> node,
        TypeInformation type
    )
    {
        foreach (var treeNode in node.Nodes)
        {
            if (treeNode.Value.Outputs.Span.Contains(type))
            {
                return treeNode.Value.GetStatus(type);
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
}
