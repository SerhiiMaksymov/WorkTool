namespace WorkTool.Core.Modules.Expressions.Extensions;

public static class ExpressionExtension
{
    public static BinaryExpression Assign(this Expression expression, Expression right)
    {
        return Expression.Assign(expression, right);
    }

    public static MemberExpression Property(this Expression expression, PropertyInfo property)
    {
        return Expression.Property(expression, property);
    }

    public static LambdaExpression Lambda(this Expression expression)
    {
        return Expression.Lambda(expression);
    }

    public static UnaryExpression Convert(this Expression expression, Type type)
    {
        return Expression.Convert(expression, type);
    }

    public static LambdaExpression Lambda(
        this Expression expression,
        params ParameterExpression[] parameters
    )
    {
        return Expression.Lambda(expression, parameters);
    }
}
