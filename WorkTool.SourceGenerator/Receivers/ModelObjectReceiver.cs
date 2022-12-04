namespace WorkTool.SourceGenerator.Receivers;

public class ModelObjectReceiver : ISyntaxReceiver
{
    public readonly List<ModelObjectParameters> Items = new ();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not AttributeSyntax attribute)
        {
            return;
        }

        if (!nameof(ModelObjectAttribute).Contains(attribute.Name.ToString()))
        {
            return;
        }

        if (attribute.ArgumentList is null)
        {
            return;
        }

        if (attribute.ArgumentList.Arguments.Count != 1)
        {
            return;
        }

        if (attribute.ArgumentList.Arguments[0].Expression is not TypeOfExpressionSyntax typeExpressionSyntax)
        {
            return;
        }

        Items.Add(new ModelObjectParameters(typeExpressionSyntax.Type));
    }
}