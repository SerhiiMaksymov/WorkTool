namespace WorkTool.Core.Modules.Expressions.Extensions;

public static class ConstructorInfoExtension
{
    public static NewExpression ToNew(this ConstructorInfo constructor)
    {
        return Expression.New(constructor);
    }

    public static NewExpression ToNew(
        this ConstructorInfo constructor,
        IEnumerable<Expression> expressions
    )
    {
        return Expression.New(constructor, expressions);
    }

    public static NewExpression ToNew(this ConstructorInfo constructor, ExpressionScope scope)
    {
        return constructor.ToNew(scope.Expressions.ToArray());
    }
}
