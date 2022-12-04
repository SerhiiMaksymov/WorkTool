namespace WorkTool.SourceGenerator.Models;

public readonly struct PropertyGetterOptions
{
    public PropertyGetterOptions(AccessModifier accessModifier, string? body)
    {
        AccessModifier = accessModifier;
        Body           = body;
    }

    public AccessModifier AccessModifier { get; }
    public string?        Body           { get; }

    private string GetBody()
    {
        if (string.IsNullOrWhiteSpace(Body))
        {
            return ";";
        }

        return $@"
{{
    {
        Body
    }
}}";
    }

    public override string ToString()
    {
        var items = new List<string>();

        if (AccessModifier != AccessModifier.NotApplicable && AccessModifier != AccessModifier.Public)
        {
            items.Add(AccessModifier.AsString());
        }

        items.Add($"get{GetBody()}");

        return items.JoinString(" ");
    }
}