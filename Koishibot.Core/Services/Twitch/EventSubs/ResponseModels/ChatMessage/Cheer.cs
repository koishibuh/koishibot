using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage;

public class Cheer
{
    ///<summary>
    ///The amount of Bits the user cheered.
    ///</summary>
    [JsonPropertyName("bits")]
    public int Bits { get; set; }
}