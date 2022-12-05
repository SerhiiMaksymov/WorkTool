namespace WorkTool.SourceGenerator.Extensions;

public static class StringExtension
{
    public static string OptionsTypeNameToModelTypeName(this string optionsTypeName)
    {
        return optionsTypeName.EndsWith("Options")
            ? optionsTypeName.Substring(0, optionsTypeName.Length - 7)
            : optionsTypeName;
    }

    public static NamespaceOptions ToNamespace(this string str)
    {
        return new NamespaceOptions(str.ToNamespaceSegments());
    }

    public static string PropertyNameToFieldName(this string propertyName)
    {
        return $"_{propertyName[0].ToString().ToLower()}{propertyName.Substring(1)}";
    }

    public static string PropertyNameToArgumentName(this string propertyName)
    {
        return $"{propertyName[0].ToString().ToLower()}{propertyName.Substring(1)}";
    }

    public static string PropertyNameToSingle(this string propertyName)
    {
        if (propertyName == "Children")
        {
            return "Child";
        }

        if (propertyName.EndsWith("ses"))
        {
            return propertyName.Substring(0, propertyName.Length - 2);
        }

        if (propertyName.EndsWith("ies"))
        {
            return $"{propertyName.Substring(0, propertyName.Length - 3)}y";
        }

        if (propertyName.EndsWith("s"))
        {
            return propertyName.Substring(0, propertyName.Length - 1);
        }

        return propertyName;
    }

    public static IEnumerable<string> GetAttributeNames(this string fullName)
    {
        yield return fullName;
        yield return fullName.Substring(0, fullName.Length - 9);
    }

    public static IEnumerable<NamespaceSegment> ToNamespaceSegments(this string str)
    {
        foreach (var segment in str.Split('.'))
        {
            yield return new NamespaceSegment(segment);
        }
    }
}
