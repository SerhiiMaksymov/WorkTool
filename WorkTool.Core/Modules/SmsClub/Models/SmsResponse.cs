namespace WorkTool.Core.Modules.SmsClub.Models;

public class SmsResponse<TSuccessRequest>
{
    public const string SuccessRequestJsonPropertyName = "success_request";

    [JsonPropertyName(SuccessRequestJsonPropertyName)]
    public TSuccessRequest? SuccessRequest { get; set; }
}
