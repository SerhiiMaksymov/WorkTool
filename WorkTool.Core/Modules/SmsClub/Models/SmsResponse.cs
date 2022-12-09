namespace WorkTool.Core.Modules.SmsClub.Models;

public class SmsResponse<TSuccessRequest>
{
    [JsonPropertyName("success_request")]
    public TSuccessRequest? SuccessRequest { get; set; }
}
