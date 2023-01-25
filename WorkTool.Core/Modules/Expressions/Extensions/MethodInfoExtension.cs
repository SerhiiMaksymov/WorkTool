namespace WorkTool.Core.Modules.Expressions.Extensions;

public static class MethodInfoExtension
{
    public static bool IsRTDynamicMethod(this MethodInfo methodInfo)
    {
        if (
            methodInfo.GetType().ToString()
            == "System.Reflection.Emit.DynamicMethod+RTDynamicMethod"
        )
        {
            return true;
        }

        return false;
    }

    public static MethodCallExpression ToCall(
        this MethodInfo method,
        Expression? instance,
        params Expression[] arguments
    )
    {
        return method.ToCall(instance, arguments.AsEnumerable());
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
