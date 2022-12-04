namespace WorkTool.SourceGenerator.Receivers;

public class OptionsObjectReceiver : ISyntaxReceiver
{
    public readonly List<OptionsObjectParameters> Items = new ();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not AttributeSyntax attribute)
        {
            return;
        }

        if (!nameof(OptionsObjectAttribute).Contains(attribute.Name.ToString()))
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

        if (attribute.ArgumentList.Arguments[0].Expression is not TypeOfExpressionSyntax typeExpressionSyntax)
        {
            return;
        }

        if (attribute.ArgumentList.Arguments[1].Expression is not LiteralExpressionSyntax name)
        {
            return;
        }

        Items.Add(
            new OptionsObjectParameters(
                typeExpressionSyntax.Type,
                name.Token.ValueText == "null" ? null : name.Token.ValueText));
    }
}