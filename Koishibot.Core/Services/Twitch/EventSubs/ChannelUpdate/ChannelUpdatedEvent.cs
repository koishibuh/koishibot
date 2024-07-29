using Koishibot.Core.Services.Twitch.Enums;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelUpdate;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelupdate">Twitch Documentation</see><br/>
///  When a broadcaster updates the category, title, content classification labels, or broadcast language for their channel.<br/>
///  Required scopes: None.
/// </summary>
public class ChannelUpdatedEvent
{
	/// <summary>
	///	The broadcaster’s user ID.
	/// </summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string BroadcasterId { get; set; } = string.Empty;

	/// <summary>
	/// The broadcaster’s user login. (Lowercase)
	/// </summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string BroadcasterLogin { get; set; } = string.Empty;

	/// <summary>
	/// The broadcaster’s user display name.
	/// </summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string BroadcasterName { get; set; } = string.Empty;

	/// <summary>
	/// The channel’s stream title.
	/// </summary>
	[JsonPropertyName("title")]
	public string StreamTitle { get; set; } = string.Empty;

	/// <summary>
	/// The channel’s broadcast language.
	/// </summary>
	[JsonPropertyName("language")]
	public string Language { get; set; } = string.Empty;

	/// <summary>
	/// The channel’s category ID.
	/// </summary>
	[JsonPropertyName("category_id")]
	public string CategoryId { get; set; } = string.Empty;

	/// <summary>
	/// The category name.
	/// </summary>
	[JsonPropertyName("category_name")]
	public string CategoryName { get; set; } = string.Empty;

	/// <summary>
	/// Array of content classification label IDs currently applied on the Channel.<br/>
	/// To retrieve a list of all possible IDs, use the Get Content Classification Labels API endpoint.
	/// </summary>
	[JsonPropertyName("content_classification_labels")]
	public List<ContentClassificationLabel> ContentClassificationLabels { get; set; } = [];
}