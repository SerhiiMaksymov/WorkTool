namespace WorkTool.Core.Modules.Expressions.Extensions;

public static class TypeExtension
{
    public static NewExpression ToNew(this Type type)
    {
        return Expression.New(type);
    }

    public static ParameterExpression ToVariable(this Type type, string name)
    {
        return Expression.Variable(type, name);
    }

    public static LabelTarget ToLabel(this Type type)
    {
        return Expression.Label(type);
    }

    public static NewArrayExpression ToNewArrayInit(this Type type)
    {
        return Expression.NewArrayInit(type);
    }

    public static NewArrayExpression ToNewArrayInit(
        this Type type,
        IEnumerable<Expression> expressions
    )
    {
        return Expression.NewArrayInit(type, expressions);
    }
}
