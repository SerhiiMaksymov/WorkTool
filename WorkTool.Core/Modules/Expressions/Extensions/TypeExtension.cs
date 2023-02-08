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

    public static ParameterExpression ToVariable(this Type type)
    {
        return Expression.Variable(type);
    }

    public static ParameterExpression ToVariableAutoName(this Type type)
    {
        return type.ToVariable(RandomStringGuid.Digits.GetRandom());
    }

    public static ParameterExpression ToParameter(this Type type, string name)
    {
        return Expression.Parameter(type, name);
    }

    public static ParameterExpression ToParameter(this Type type)
    {
        return Expression.Parameter(type);
    }

    public static ParameterExpression ToParameterAutoName(this Type type)
    {
        return type.ToParameter(RandomStringGuid.Digits.GetRandom());
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
