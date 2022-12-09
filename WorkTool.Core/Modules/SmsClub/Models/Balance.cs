namespace WorkTool.Core.Modules.SmsClub.Models;

public class Balance
{
    [JsonPropertyName("money")]
    public double Money { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }
}
