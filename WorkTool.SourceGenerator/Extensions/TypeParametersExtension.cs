namespace WorkTool.SourceGenerator.Extensions;

public static class TypeParametersExtension
{
    public static string ToText(this TypeParameters parameters)
    {
        if (parameters.Namespace is null)
        {
            return parameters.Name;
        }

        if (parameters.IsPrimitiveType())
        {
            return parameters.ToString();
        }

        return $"global::{parameters}";
    }
}
