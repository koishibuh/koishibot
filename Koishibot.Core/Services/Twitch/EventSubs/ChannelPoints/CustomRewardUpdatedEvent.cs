using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Converters;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardupdate">Twitch Documentation</see><br/>
/// When a custom channel points reward has been updated for the specified channel.<br/>
/// Required Scopes: channel:read:redemptions or channel:manage:redemptions<br/>
/// </summary>
public class CustomRewardUpdatedEvent
{
	///<summary>
	///The reward identifier.
	///</summary>
	[JsonPropertyName("id")]
	public string RewardId { get; set; } = string.Empty;

	///<summary>
	///The requested broadcaster ID.
	///</summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string BroadcasterId { get; set; } = string.Empty;

	///<summary>
	///The requested broadcaster login.
	///</summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string BroadcasterLogin { get; set; } = string.Empty;

	///<summary>
	///The requested broadcaster display name.
	///</summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string BroadcasterName { get; set; } = string.Empty;

	///<summary>
	///Is the reward currently enabled. If false, the reward won’t show up to viewers.
	///</summary>
	[JsonPropertyName("is_enabled")]
	public bool IsEnabled { get; set; }

	///<summary>
	///Is the reward currently paused. If true, viewers can’t redeem.
	///</summary>
	[JsonPropertyName("is_paused")]
	public bool IsPaused { get; set; }

	///<summary>
	///Is the reward currently in stock. If false, viewers can’t redeem.
	///</summary>
	[JsonPropertyName("is_in_stock")]
	public bool IsInStock { get; set; }

	///<summary>
	///The reward title.
	///</summary>
	[JsonPropertyName("title")]
	public string? Title { get; set; }

	///<summary>
	///The reward cost.
	///</summary>
	[JsonPropertyName("cost")]
	public int Cost { get; set; }

	///<summary>
	///The reward description.
	///</summary>
	[JsonPropertyName("prompt")]
	public string? Description { get; set; }

	///<summary>
	///Does the viewer need to enter information when redeeming the reward.
	///</summary>
	[JsonPropertyName("is_user_input_required")]
	public bool IsUserInputRequired { get; set; }

	///<summary>
	///Should redemptions be set to fulfilled status immediately when redeemed and skip the request queue instead of the normal unfulfilled status.
	///</summary>
	[JsonPropertyName("should_redemptions_skip_request_queue")]
	public bool ShouldRedemptionsSkipRequestQueue { get; set; }

	///<summary>
	///Whether a maximum per stream is enabled and what the maximum is.
	///</summary>
	[JsonPropertyName("max_per_stream")]
	public MaxPerStream? MaxPerStream { get; set; }

	///<summary>
	///Whether a maximum per user per stream is enabled and what the maximum is.
	///</summary>
	[JsonPropertyName("max_per_user_per_stream")]
	public MaxPerUserPerStream? MaxPerUserPerStream { get; set; }

	///<summary>
	///Custom background color for the reward. Format: Hex with # prefix. Example: #FA1ED2.
	///</summary>
	[JsonPropertyName("background_color")]
	public string? BackgroundColor { get; set; }

	///<summary>
	///Set of custom images of 1x, 2x and 4x sizes for the reward. Can be null if no images have been uploaded.
	///</summary>
	[JsonPropertyName("image")]
	public ImageSizes? CustomImage { get; set; }

	///<summary>
	///Set of default images of 1x, 2x and 4x sizes for the reward.
	///</summary>
	[JsonPropertyName("default_image")]
	public ImageSizes? DefaultImage { get; set; }

	///<summary>
	///Whether a cooldown is enabled and what the cooldown is in seconds.
	///</summary>
	[JsonPropertyName("global_cooldown")]
	public GlobalCooldown? GlobalCooldown { get; set; }

	///<summary>
	///Timestamp of the cooldown expiration. null if the reward isn’t on cooldown.br/>
	///(Converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("cooldown_expires_at")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset CooldownExpiresAt { get; set; }

	///<summary>
	///The number of redemptions redeemed during the current live stream.<br/>
	///Counts against the max_per_stream limit.<br/>
	///Null if the broadcasters stream isn’t live or max_per_stream isn’t enabled.
	///</summary>
	[JsonPropertyName("redemptions_redeemed_current_stream")]
	public int? RedemptionsRedeemedCurrentStream { get; set; }
}