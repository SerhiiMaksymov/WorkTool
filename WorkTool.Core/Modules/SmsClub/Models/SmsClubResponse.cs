using System.Text.Json.Serialization;

namespace WorkTool.Core.Modules.SmsClub.Models;

public class SmsClubResponse
{
    [JsonPropertyName("success_request")]
    public SuccessRequest? SuccessRequest { get; set; }
}
