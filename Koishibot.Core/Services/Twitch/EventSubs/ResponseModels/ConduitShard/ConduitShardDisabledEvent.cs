using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ConduitShard;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#conduitsharddisabled">Twitch Documentation</see><br/>
/// When EventSub disables a shard due to the status of the underlying transport changing.<br/>
/// Required Scopes: Check documentation for list<br/>
/// </summary>
public class ConduitShardDisabledEvent
{
    ///<summary>
    ///The ID of the conduit.
    ///</summary>
    [JsonPropertyName("conduit_id")]
    public string ConduitId { get; set; }

    ///<summary>
    ///The ID of the disabled shard.
    ///</summary>
    [JsonPropertyName("shard_id")]
    public string ShardId { get; set; }

    ///<summary>
    ///The new status of the transport.
    ///</summary>
    [JsonPropertyName("status")]
    public string Status { get; set; }

    ///<summary>
    ///The disabled transport.
    ///</summary>
    [JsonPropertyName("transport")]
    public Transport Transport { get; set; }
}