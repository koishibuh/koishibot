using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.User;

public class Whisper
{
    /// <summary>
    /// The body of the whisper message.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; }
}