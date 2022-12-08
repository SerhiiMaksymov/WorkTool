namespace WorkTool.Core.Modules.SmsClub.Models;

public class GetSmsStatusRequest
{
    [JsonPropertyName("id_sms")]
    public string[]? SmsIds { get; set; }
}
