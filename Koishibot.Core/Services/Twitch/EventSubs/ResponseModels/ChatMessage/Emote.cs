using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage;

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

    ///<summary>
    ///The ID of the broadcaster who owns the emote.
    ///</summary>
    [JsonPropertyName("owner_id")]
    public string OwnerId { get; set; }

    ///<summary>
    ///The formats that the emote is available in.<br/>
    ///For example, if the emote is available only as a static PNG, the array contains only static.<br/>
    ///If the emote is available as a static PNG and an animated GIF, the array contains static and animated.
    ///</summary>
    [JsonPropertyName("format")]
    public EmoteFormat Format { get; set; }
}