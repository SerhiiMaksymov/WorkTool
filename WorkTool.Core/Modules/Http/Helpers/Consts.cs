namespace WorkTool.Core.Modules.Http.Helpers;

public static class Consts
{
    public const string AuthenticationHeaderBearerName = "Bearer";

    public static readonly ReadOnlyMemory<HttpStatusCode> ErrorHttpStatusCodes =
        Enum.GetValues<HttpStatusCode>()
            .Where(x => x < (HttpStatusCode)200 || x >= (HttpStatusCode)300)
            .ToArray();
}
