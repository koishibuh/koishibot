using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.RequestModels;

public class EventMessage<T> where T : class
{
    [JsonPropertyName("metadata")]
    public Metadata Metadata { get; set; } = null!;

    [JsonPropertyName("payload")]
    public Payload<T>? Payload { get; set; }
}