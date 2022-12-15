namespace WorkTool.Core.Modules.SmsClub.Models;

public class ObjectSuccessRequest<TObject>
{
    [JsonPropertyName("info")]
    public TObject? Object { get; set; }
}
