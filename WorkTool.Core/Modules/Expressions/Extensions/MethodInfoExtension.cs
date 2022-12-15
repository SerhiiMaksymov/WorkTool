namespace WorkTool.Core.Modules.Expressions.Extensions;

public static class MethodInfoExtension
{
    public static MethodCallExpression ToCall(
        this MethodInfo method,
        Expression? instance,
        params Expression[] arguments
    )
    {
        return Expression.Call(instance, method, arguments);
    }

    public static MethodCallExpression ToCall(
        this MethodInfo method,
        Expression? instance,
        IEnumerable<Expression> arguments
    )
    {
        return Expression.Call(instance, method, arguments);
    }
}
