namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class ExpressionExtension
{
    public static string GetBindingPath(this Expression expression)
    {
        var lambdaExpression = (LambdaExpression)expression;
        var memberExpression = (MemberExpression)lambdaExpression.Body;
        var stringBuilder    = new StringBuilder();
        GetMemberPath(memberExpression, stringBuilder);
        var result = stringBuilder.ToString();

        return result;
    }

    private static void GetMemberPath(MemberExpression memberExpression, StringBuilder stringBuilder)
    {
        if (memberExpression.Expression is MemberExpression parent)
        {
            GetMemberPath(parent, stringBuilder);
            stringBuilder.Append($".{memberExpression.Member.Name}");
        }
        else
        {
            stringBuilder.Append(memberExpression.Member.Name);
        }
    }
}