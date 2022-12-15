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
}
