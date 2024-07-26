using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications;

public class Badge
{
    /// <summary>
    /// An ID that identifies this set of chat badges. For example, Bits or Subscriber.
    /// </summary>
    [JsonPropertyName("set_id")]
    public string SetId { get; set; }

    /// <summary>
    /// An ID that identifies this version of the badge. The ID can be any value.<br/>
    /// For example, for Bits, the ID is the Bits tier level, but for World of Warcraft, it could be Alliance or Horde.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// Contains metadata related to the chat badges in the badges tag. <br/>
    /// Currently, this tag contains metadata only for subscriber badges, to indicate the number of months the user has been a subscriber.
    /// </summary>
    [JsonPropertyName("info")]
    public string Info { get; set; }
}