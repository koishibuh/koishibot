using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.SuspiciousUser;
public class Emote
{
    ///<summary>
    ///An ID that uniquely identifies this emote.
    ///</summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    ///<summary>
    ///An ID that identifies the emote set that the emote belongs to.
    ///</summary>
    [JsonPropertyName("emote_set_id")]
    public string EmoteSetId { get; set; }
}