namespace WorkTool.Core.Modules.Http.Models;

public class AuthenticationBearerHeaderValue : AuthenticationHeaderValue
{
    public AuthenticationBearerHeaderValue(string parameter)
        : base(Consts.AuthenticationHeaderBearerName, parameter) { }
}
