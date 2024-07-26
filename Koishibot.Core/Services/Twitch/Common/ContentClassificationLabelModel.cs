using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.Common;
public class ContentClassificationLabelModel
{
    ///<summary>
    ///ID of the Content Classification Labels that must be added/removed from the channel.
    ///</summary>
    [JsonPropertyName("id")]
    [JsonConverter(typeof(ContentClassificationLabelEnumConverter))]
    public ContentClassificationLabel Id { get; set; }

    ///<summary>
    ///Boolean flag indicating whether the label should be enabled (true) or disabled for the channel.
    ///</summary>
    [JsonPropertyName("is_enabled")]
    public bool IsEnabled { get; set; }
}
