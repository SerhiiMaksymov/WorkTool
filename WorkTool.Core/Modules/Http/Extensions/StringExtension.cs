namespace WorkTool.Core.Modules.Http.Extensions;

public static class StringExtension
{
    public static AuthenticationBearerHeaderValue ToAuthenticationBearer(this string str)
    {
        return new AuthenticationBearerHeaderValue(str);
    }
}
