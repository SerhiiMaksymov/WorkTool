namespace WorkTool.SourceGenerator.Extensions;

public static class AccessibilityExtension
{
    public static AccessModifier ToAccessModifier(this Accessibility accessibility)
    {
        return accessibility switch
        {
            Accessibility.NotApplicable => AccessModifier.NotApplicable,
            Accessibility.Private => AccessModifier.Private,
            Accessibility.ProtectedAndInternal => AccessModifier.ProtectedInternal,
            Accessibility.Protected => AccessModifier.Protected,
            Accessibility.Internal => AccessModifier.Internal,
            Accessibility.ProtectedOrInternal => AccessModifier.ProtectedInternal,
            Accessibility.Public => AccessModifier.Public,
            _ => throw new ArgumentOutOfRangeException(nameof(accessibility), accessibility, null)
        };
    }
}
