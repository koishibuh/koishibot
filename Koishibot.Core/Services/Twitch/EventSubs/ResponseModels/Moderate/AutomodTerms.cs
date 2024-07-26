using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderate.Enums;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderate;

public class AutomodTerms
{
    ///<summary>
    ///Either “add” or “remove”.
    ///</summary>
    [JsonPropertyName("action")]
    public AutoModTermAction Action { get; set; }

    ///<summary>
    ///Either “blocked” or “permitted”.
    ///</summary>
    [JsonPropertyName("list")]
    public TermListType List { get; set; }

    ///<summary>
    ///Terms being added or removed.
    ///</summary>
    [JsonPropertyName("terms")]
    public List<string> Terms { get; set; }

    ///<summary>
    ///Whether the terms were added due to an Automod message approve/deny action.
    ///</summary>
    [JsonPropertyName("from_automod")]
    public bool FromAutomod { get; set; }
}
