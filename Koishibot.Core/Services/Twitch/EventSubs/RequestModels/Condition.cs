using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.RequestModels;

public class Condition
{
    [JsonPropertyName("broadcaster_user_id")]
    public string? BroadcasterUserId { get; set; }

    [JsonPropertyName("broadcaster_id")]
    public string? BroadcasterId { get; set; }

    [JsonPropertyName("moderator_user_id")]
    public string? ModeratorUserId { get; set; }

    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

    [JsonPropertyName("to_broadcaster_user_id")]
    public string? ToBroadcasterUserId { get; set; }

    [JsonPropertyName("from_broadcaster_user_id")]
    public string? FromBroadcasterUserId { get; set; }

    [JsonPropertyName("reward_id")]
    public string? RewardId { get; set; }
}