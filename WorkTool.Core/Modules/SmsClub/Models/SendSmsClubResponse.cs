using System.Text.Json.Serialization;

namespace WorkTool.Core.Modules.SmsClub.Models;

public class SendSmsClubResponse
{
    [JsonPropertyName("success_request")]
    public SuccessRequest SuccessRequest { get; set; }
}
