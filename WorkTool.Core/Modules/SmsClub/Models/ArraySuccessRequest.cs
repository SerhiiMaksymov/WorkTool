namespace WorkTool.Core.Modules.SmsClub.Models;

public class ArraySuccessRequest
{
    [JsonPropertyName("info")]
    public string[]? Info { get; set; }
}
