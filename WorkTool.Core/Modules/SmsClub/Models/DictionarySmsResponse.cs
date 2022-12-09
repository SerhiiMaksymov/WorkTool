namespace WorkTool.Core.Modules.SmsClub.Models;

public class DictionarySmsResponse
{
    [JsonPropertyName("success_request")]
    public DictionarySuccessRequest? SuccessRequest { get; set; }
}
