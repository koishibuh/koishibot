using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.RequestModels;

public class Payload<T> where T : class
{
    [JsonPropertyName("session")]
    public Session? Session { get; set; }

    [JsonPropertyName("subscription")]
    public Subscription? Subscription { get; set; }

    [JsonPropertyName("event")]
    public T? Event { get; set; }

}
