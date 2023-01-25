namespace WorkTool.Core.Modules.DependencyInjection.Models;

public readonly struct DependencyStatus
{
    public DependencyStatus(
        TypeInformation type,
        ReadOnlyMemory<ParameterInfo> neededParameters,
        Delegate @delegate,
        ReadOnlyDictionary<ParameterInfo, Expression> parameterValues
    )
    {
        NeededParameters = neededParameters;
        Delegate = @delegate;
        ParameterValues = parameterValues;
        Type = type;
    }

    public TypeInformation Type { get; }
    public ReadOnlyMemory<ParameterInfo> NeededParameters { get; }
    public Delegate Delegate { get; }
    public ReadOnlyDictionary<ParameterInfo, Expression> ParameterValues { get; }

    public Expression CreateExpression(ReadOnlyDictionary<ParameterInfo, Expression> parameters)
    {
        var expressions = new List<Expression>();
        var methodParameters = Delegate.Method.GetParameters();

        foreach (var methodParameter in methodParameters)
        {
            if (methodParameter.ParameterType.IsClosure())
            {
                continue;
            }

            if (ParameterValues.TryGetValue(methodParameter, out var expression))
            {
                expressions.Add(expression);

                continue;
            }

            if (parameters.TryGetValue(methodParameter, out var parameter))
            {
                expressions.Add(parameter);

                continue;
            }

            throw new Exception();
        }

        return Delegate.ToCall(expressions);
    }
}
