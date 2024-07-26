using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications;

public class Fragments
{
    /// <summary>
    /// The type of message fragment.
    /// </summary>
    [JsonPropertyName("type")]
    public FragmentType Type { get; set; }

    /// <summary>
    /// Message text in fragment.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; }
}