namespace WorkTool.Core.Modules.DependencyInjection.Models;

public readonly struct ExpressionScope
{
    public ExpressionScope(
        ReadOnlyMemory<Expression> expressions,
        ReadOnlyMemory<ParameterExpression> variables
    )
    {
        Expressions = expressions;
        Variables = variables;
    }

    public ReadOnlyMemory<Expression> Expressions { get; }
    public ReadOnlyMemory<ParameterExpression> Variables { get; }
}
