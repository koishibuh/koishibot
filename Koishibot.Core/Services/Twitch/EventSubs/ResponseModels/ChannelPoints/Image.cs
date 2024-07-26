using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#image">Twitch Documentation</see>
/// </summary>
public class Image
{
    ///<summary>
    ///URL for the image at 1x size.
    ///</summary>
    [JsonPropertyName("url_1x")]
    public string Url1X { get; set; }

    ///<summary>
    ///URL for the image at 2x size.
    ///</summary>
    [JsonPropertyName("url_2x")]
    public string Url2X { get; set; }

    ///<summary>
    ///URL for the image at 4x size.
    ///</summary>
    [JsonPropertyName("url_4x")]
    public string Url4X { get; set; }
}