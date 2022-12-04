namespace WorkTool.SourceGenerator.Extensions;

public static class PropertyParametersExtension
{
    public static IEnumerable<ArgumentParameters> ToArguments(this IEnumerable<PropertyParameters> properties)
    {
        foreach (var property in properties)
        {
            yield return new ArgumentParameters(
                false,
                property.Type,
                property.Name.PropertyNameToArgumentName());
        }
    }
}