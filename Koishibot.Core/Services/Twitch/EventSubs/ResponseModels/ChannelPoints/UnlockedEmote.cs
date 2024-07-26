using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;

public class UnlockedEmote
{
    /// <summary>
    /// The emote ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// The human readable emote token.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}