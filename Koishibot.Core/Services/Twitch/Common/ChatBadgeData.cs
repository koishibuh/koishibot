using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.Common;
public class ChatBadgeData
{
	///<summary>
	///An ID that identifies this set of chat badges. For example, Bits or Subscriber.
	///</summary>
	[JsonPropertyName("set_id")]
	public string SetId { get; set; }

	///<summary>
	///The list of chat badges in this set.
	///</summary>
	[JsonPropertyName("versions")]
	public List<ChatBadgeVersion> Versions { get; set; }
}

public class ChatBadgeVersion
{
	///<summary>
	///An ID that identifies this version of the badge.<br/>
	///The ID can be any value. For example, for Bits, the ID is the Bits tier level, but for World of Warcraft, it could be Alliance or Horde.
	///</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	///<summary>
	///A URL to the small version (18px x 18px) of the badge.
	///</summary>
	[JsonPropertyName("image_url_1x")]
	public string ImageUrl_18px { get; set; }

	///<summary>
	///A URL to the medium version (36px x 36px) of the badge.
	///</summary>
	[JsonPropertyName("image_url_2x")]
	public string ImageUrl_36px { get; set; }

	///<summary>
	///A URL to the large version (72px x 72px) of the badge.
	///</summary>
	[JsonPropertyName("image_url_4x")]
	public string ImageUrl_72px { get; set; }

	///<summary>
	///The title of the badge.
	///</summary>
	[JsonPropertyName("title")]
	public string Title { get; set; }

	///<summary>
	///The description of the badge.
	///</summary>
	[JsonPropertyName("description")]
	public string Description { get; set; }

	///<summary>
	///The action to take when clicking on the badge. Set to null if no action is specified.
	///</summary>
	[JsonPropertyName("click_action")]
	public string ClickAction { get; set; }

	///<summary>
	///The URL to navigate to when clicking on the badge. Set to null if no URL is specified.
	///</summary>
	[JsonPropertyName("click_url")]
	public string ClickUrl { get; set; }
}
