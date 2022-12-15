namespace WorkTool.Core.Modules.SmsClub.Models;

public class DictionarySuccessRequest
{
    public const string InfoJsonPropertyName = "info";

    [JsonPropertyName(InfoJsonPropertyName)]
    public Dictionary<string, string>? Info { get; set; }

    [JsonPropertyName("add_info")]
    public Dictionary<string, string>? AddInfo { get; set; }
}
