namespace WorkTool.Core.Modules.SmsClub.Models;

public class ArraySmsResponse
{
    [JsonPropertyName("success_request")]
    public ArraySuccessRequest? SuccessRequest { get; set; }
}
