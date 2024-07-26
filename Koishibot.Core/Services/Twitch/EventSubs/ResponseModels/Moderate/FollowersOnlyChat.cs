using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderate;

public class FollowersOnlyChat
{
    ///<summary>
    ///The length of time, in minutes, that the followers must have followed the broadcaster to participate in the chat room.
    ///</summary>
    [JsonPropertyName("follow_duration_minutes")]
    public int FollowDurationMinutes { get; set; }
}