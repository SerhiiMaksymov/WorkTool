namespace WorkTool.SourceGenerator.Extensions;

public static class AccessModifierExtension
{
    public static string AsString(this AccessModifier modifier)
    {
        return modifier switch
        {
            AccessModifier.Public => "public",
            AccessModifier.Protected => "protected",
            AccessModifier.Internal => "internal",
            AccessModifier.ProtectedInternal => "protected internal",
            AccessModifier.Private => "private",
            AccessModifier.PrivateProtected => "private protected",
            _ => throw new ArgumentOutOfRangeException(nameof(modifier), modifier.ToString())
        };
    }
}