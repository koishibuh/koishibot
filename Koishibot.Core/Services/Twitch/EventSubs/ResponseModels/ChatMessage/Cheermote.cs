using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage;

public class Cheermote
{
    ///<summary>
    ///The name portion of the Cheermote string that you use in chat to cheer Bits. <br/>
    ///For example, if the prefix is Cheer and you want to cheer 100 Bits, the full Cheermote string is Cheer100.<br/>
    ///When the Cheermote string is entered in chat, Twitch converts it to the image associated with the Bits tier that was cheered.<br/>
    ///The full Cheermote string is the concatenation of prefix  number of Bits. 
    ///</summary>
    [JsonPropertyName("prefix")]
    public string? Prefix { get; set; }

    ///<summary>
    ///The amount of bits cheered.
    ///</summary>
    [JsonPropertyName("bits")]
    public int Bits { get; set; }

    ///<summary>
    ///The tier level of the cheermote.
    ///</summary>
    [JsonPropertyName("tier")]
    public int Tier { get; set; }
}