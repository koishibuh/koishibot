using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderate;

public class SlowChat
{
    ///<summary>
    ///The amount of time, in seconds, that users need to wait between sending messages.
    ///</summary>
    [JsonPropertyName("wait_time_seconds")]
    public int WaitTimeSeconds { get; set; }
}