namespace WorkTool.SourceGenerator.Receivers;

public class PartObjectReceiver : ISyntaxReceiver
{
    public readonly List<PartObjectParameters> Items = new ();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not AttributeSyntax attribute)
        {
            return;
        }

        if (!nameof(PartObjectAttribute).Contains(attribute.Name.ToString()))
        {
            return;
        }

        if (attribute.ArgumentList is null)
        {
            return;
        }

        if (attribute.ArgumentList.Arguments.Count != 4)
        {
            return;
        }

        if (attribute.ArgumentList.Arguments[0].Expression is not TypeOfExpressionSyntax typeExpressionSyntax)
        {
            return;
        }

        if (attribute.ArgumentList.Arguments[1].Expression is not LiteralExpressionSyntax nameSyntax)
        {
            return;
        }

        if (attribute.ArgumentList.Arguments[2].Expression is not LiteralExpressionSyntax classNameSyntax)
        {
            return;
        }

        if (attribute.ArgumentList.Arguments[3].Expression is not MemberAccessExpressionSyntax objectTypeSyntax)
        {
            return;
        }

        var objectType = Enum.Parse<ObjectType>(objectTypeSyntax.Name.Identifier.ValueText);

        Items.Add(
            new PartObjectParameters(
                typeExpressionSyntax.Type,
                nameSyntax.Token.ValueText,
                classNameSyntax.Token.ValueText,
                objectType));
    }
}