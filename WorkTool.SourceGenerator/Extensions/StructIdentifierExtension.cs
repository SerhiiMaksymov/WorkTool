namespace WorkTool.SourceGenerator.Extensions;

public static class StructIdentifierExtension
{
    public static string AsString(this StructIdentifier identifier)
    {
        switch (identifier)
        {
            case StructIdentifier.None:     return string.Empty;
            case StructIdentifier.Readonly: return "readonly";
            case StructIdentifier.Partial:  return "partial";
            default:                        throw new ArgumentOutOfRangeException(nameof(StructIdentifier));
        }
    }
}