using System.Text.Json.Serialization;

namespace WorkTool.Core.Modules.SmsClub.Models;

public class SendSmsClubRequest
{
    [JsonPropertyName("phone")]
    public string[]? PhoneNumbers { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("src_addr")]
    public string? Recipient { get; set; }
}
