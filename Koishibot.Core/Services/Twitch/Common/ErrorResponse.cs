namespace Koishibot.Core.Services.Twitch.Common;

public class ErrorResponse
{
    [JsonPropertyName("error")]
    public string Error { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}