namespace WorkTool.SourceGenerator.Receivers;

public class FluentObjectReceiver : ISyntaxReceiver
{
    public readonly List<FluentObjectParameters> Items = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not AttributeSyntax attribute)
        {
            return;
        }

        if (!nameof(FluentObjectAttribute).Contains(attribute.Name.ToString()))
        {
            return;
        }

        if (attribute.ArgumentList is null)
        {
            return;
        }

        if (attribute.ArgumentList.Arguments.Count != 2)
        {
            return;
        }

        if (
            attribute.ArgumentList.Arguments[0].Expression
            is not TypeOfExpressionSyntax typeExpressionSyntax
        )
        {
            return;
        }

        if (attribute.ArgumentList.Arguments[1].Expression is not LiteralExpressionSyntax name)
        {
            return;
        }

        Items.Add(new FluentObjectParameters(typeExpressionSyntax.Type, name.Token.ValueText));
    }
}
