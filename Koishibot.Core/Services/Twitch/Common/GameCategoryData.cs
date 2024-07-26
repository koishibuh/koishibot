using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.Common;

public class GameCategoryData
{
	///<summary>
	///An ID that identifies the category or game.
	///</summary>
	[JsonPropertyName("id")]
	public string GameOrCateogryId { get; set; }

	///<summary>
	///The category’s or game’s name.
	///</summary>
	[JsonPropertyName("name")]
	public string GameOrCategoryName { get; set; }

	///<summary>
	///A URL to the category’s or game’s box art.<br/>
	///You must replace the {width}x{height} placeholder with the size of image you want.
	///</summary>
	[JsonPropertyName("box_art_url")]
	public string BoxArtUrl { get; set; }

	///<summary>
	///The ID that IGDB uses to identify this game.<br/>
	///If the IGDB ID is not available to Twitch, this field is set to an empty string.
	///</summary>
	[JsonPropertyName("igdb_id")]
	public string InternetGameDatabaseId { get; set; }
}