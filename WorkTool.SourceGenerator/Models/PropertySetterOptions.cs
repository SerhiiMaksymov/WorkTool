namespace WorkTool.SourceGenerator.Models;

public readonly struct PropertySetterOptions
{
    public PropertySetterOptions(AccessModifier accessModifier, string? body)
    {
        AccessModifier = accessModifier;
        Body           = body;
    }

    public AccessModifier AccessModifier { get; }
    public string?        Body           { get; }

    public override string ToString()
    {
        var items = new List<string>();

        if (AccessModifier != AccessModifier.NotApplicable && AccessModifier != AccessModifier.Public)
        {
            items.Add(AccessModifier.AsString());
        }

        items.Add($"set{GetBody()}");

        return items.JoinString(" ");
    }

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
}