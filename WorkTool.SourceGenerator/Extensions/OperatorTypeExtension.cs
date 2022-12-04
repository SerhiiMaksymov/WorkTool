namespace WorkTool.SourceGenerator.Extensions;

public static class OperatorTypeExtension
{
    public static string AsString(this OperatorType type)
    {
        switch (type)
        {
            case OperatorType.Implicit: return "implicit";
            case OperatorType.Explicit: return "explicit";
            default:                    return string.Empty;
        }
    }
}