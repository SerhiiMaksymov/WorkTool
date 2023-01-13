namespace WorkTool.Core.Modules.DependencyInjection.Services;

public class JoinDependencyInjector : IDependencyInjector
{
    private readonly List<IDependencyInjector> dependencyInjectors;

    public JoinDependencyInjector(params IDependencyInjector[] dependencyInjectors)
        : this(dependencyInjectors.AsEnumerable()) { }

    public JoinDependencyInjector(IEnumerable<IDependencyInjector> dependencyInjectors)
    {
        this.dependencyInjectors = new(dependencyInjectors);
        Outputs = this.dependencyInjectors.SelectMany(x => x.Outputs).Distinct().ToArray();

        Inputs = this.dependencyInjectors
            .SelectMany(x => x.Inputs)
            .Distinct()
            .Where(x => !Outputs.Contains(x))
            .ToArray();
    }

    public IEnumerable<Type> Inputs  { get; }
    public IEnumerable<Type> Outputs { get; }

    public object Resolve(Type type)
    {
        if (type.IsEnumerable())
        {
            return ResolveEnumerable(type);
        }

        foreach (var dependencyInjector in dependencyInjectors)
        {
            if (dependencyInjector.Outputs.Contains(type))
            {
                return dependencyInjector.Resolve(type);
            }
        }

        throw new NotFoundException(type.ToString());
    }

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

    private object ResolveEnumerable(Type type)
    {
        var listType = typeof(List<>);
        var resultType = listType.MakeGenericType(type.GenericTypeArguments[0]);
        var list = Activator.CreateInstance(resultType).ThrowIfNull();
        var listAddRangeMethod = resultType.GetMethod(nameof(List<object>.AddRange)).ThrowIfNull();

        foreach (var dependencyInjector in dependencyInjectors)
        {
            if (!dependencyInjector.Outputs.Contains(type))
            {
                continue;
            }

            var items = dependencyInjector.Resolve(type);
            listAddRangeMethod.Invoke(list, new[] { items });
        }

        return list;
    }
}
