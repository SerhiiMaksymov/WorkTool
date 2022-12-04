namespace WorkTool.Core.Modules.Expressions;

public static class ExpressionExtension
{
    public static LambdaExpression ToLambda<TExpression>(this TExpression expression) where TExpression : Expression
    {
        return Expression.Lambda(expression);
    }

    public static LambdaExpression ToLambda<TExpression>(
    this   TExpression           expression,
    params ParameterExpression[] parameters) where TExpression : Expression
    {
        return Expression.Lambda(expression, parameters);
    }
}