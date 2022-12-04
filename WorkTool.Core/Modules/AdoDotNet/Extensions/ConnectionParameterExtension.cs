namespace WorkTool.Core.Modules.AdoDotNet.Extensions;

public static class ConnectionParameterExtension
{
    public static bool IsDefault(this ConnectionParameter parameter)
    {
        return parameter.ParameterValue.Value == parameter.Info.DefaultValue;
    }

    public static string AsString(this ConnectionParameter parameter)
    {
        return $"{parameter.Info.DefaultAlias}={parameter.ParameterValue.Value}";
    }

    public static IEnumerable<ConnectionParameter> GetNoDefaultParameters(
    this IEnumerable<ConnectionParameter> parameters)
    {
        return parameters.Where(x => x.IsDefault().IsFalse());
    }

    public static string GetConnectionString(this IEnumerable<ConnectionParameter> parameters)
    {
        return parameters.Select(x => x.AsString()).JoinString(";");
    }
}