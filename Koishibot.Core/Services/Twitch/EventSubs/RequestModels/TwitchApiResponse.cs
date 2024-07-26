using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.RequestModels;

public class TwitchApiResponse<T> where T : class
{
    [JsonPropertyName("data")]
    public List<T>? Data { get; set; }
}