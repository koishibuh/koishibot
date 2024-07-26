using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.Common;

public class ImageSizes
{
	///<summary>
	///The URL to a small version of the image.
	///</summary>
	[JsonPropertyName("url_1x")]
	public string Url1X { get; set; }

	///<summary>
	///The URL to a medium version of the image.
	///</summary>
	[JsonPropertyName("url_2x")]
	public string Url2X { get; set; }

	///<summary>
	///The URL to a large version of the image.
	///</summary>
	[JsonPropertyName("url_4x")]
	public string Url4X { get; set; }
}