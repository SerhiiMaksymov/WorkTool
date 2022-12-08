using System.Text.Json.Serialization;

namespace WorkTool.Core.Modules.SmsClub.Models;

public class SuccessRequest
{
    [JsonPropertyName("info")]
    public Dictionary<string, string>? Info { get; set; }

    [JsonPropertyName("add_info")]
    public Dictionary<string, string>? AddInfo { get; set; }
}
