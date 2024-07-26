using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.Common;
public class CustomRewardData
{
	///<summary>
	///The ID that uniquely identifies the broadcaster.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The broadcaster’s login name.
	///</summary>
	[JsonPropertyName("broadcaster_login")]
	public string BroadcasterLogin { get; set; }

	///<summary>
	///The broadcaster’s display name.
	///</summary>
	[JsonPropertyName("broadcaster_name")]
	public string BroadcasterName { get; set; }

	///<summary>
	///The ID that uniquely identifies this custom reward.
	///</summary>
	[JsonPropertyName("id")]
	public string RewardId { get; set; }

	///<summary>
	///The title of the reward.
	///</summary>
	[JsonPropertyName("title")]
	public string RewardTitle { get; set; }

	///<summary>
	///The prompt shown to the viewer when they redeem the reward if user input is required (see the is_user_input_required field).
	///</summary>
	[JsonPropertyName("prompt")]
	public string Prompt { get; set; }

	///<summary>
	///The cost of the reward in Channel Points.
	///</summary>
	[JsonPropertyName("cost")]
	public int Cost { get; set; }

	///<summary>
	///A set of custom images for the reward.<br/>
	///This field is set to null if the broadcaster didn’t upload images.
	///</summary>
	[JsonPropertyName("image")]
	public ImageSizes Image { get; set; }


	///<summary>
	///A set of default images for the reward.
	///</summary>
	[JsonPropertyName("default_image")]
	public ImageSizes DefaultImage { get; set; }


	///<summary>
	///The background color to use for the reward.<br/>
	///The color is in Hex format (for example, #00E5CB).
	///</summary>
	[JsonPropertyName("background_color")]
	public string BackgroundColor { get; set; }

	///<summary>
	///A Boolean value that determines whether the reward is enabled.<br/>
	///Is true if enabled; otherwise, false.<br/>
	///Disabled rewards aren’t shown to the user.
	///</summary>
	[JsonPropertyName("is_enabled")]
	public bool IsEnabled { get; set; }

	///<summary>
	///A Boolean value that determines whether the user must enter information when redeeming the reward.<br/>
	///Is true if the reward requires user input.
	///</summary>
	[JsonPropertyName("is_user_input_required")]
	public bool IsUserInputRequired { get; set; }

	///<summary>
	///The settings used to determine whether to apply a maximum to the number to the redemptions allowed per live stream.
	///</summary>
	[JsonPropertyName("max_per_stream_setting")]
	public MaxPerStreamSetting MaxPerStreamSetting { get; set; }

	///<summary>
	///The settings used to determine whether to apply a maximum to the number of redemptions allowed per user per live stream.
	///</summary>
	[JsonPropertyName("max_per_user_per_stream_setting")]
	public MaxPerUserPerStreamSetting MaxPerUserPerStreamSetting { get; set; }

	///<summary>
	///The settings used to determine whether to apply a cooldown period between redemptions and the length of the cooldown.
	///</summary>
	[JsonPropertyName("global_cooldown_setting")]
	public GlobalCooldownSetting GlobalCooldownSetting { get; set; }


	///<summary>
	///A Boolean value that determines whether the reward is currently paused.<br/>
	///Is true if the reward is paused. Viewers can’t redeem paused rewards.
	///</summary>
	[JsonPropertyName("is_paused")]
	public bool IsPaused { get; set; }

	///<summary>
	///A Boolean value that determines whether the reward is currently in stock.<br/>
	///Is true if the reward is in stock. Viewers can’t redeem out of stock rewards.
	///</summary>
	[JsonPropertyName("is_in_stock")]
	public bool IsInStock { get; set; }

	///<summary>
	///A Boolean value that determines whether redemptions should be set to FULFILLED status immediately when a reward is redeemed.<br/>
	///If false, status is UNFULFILLED and follows the normal request queue process.
	///</summary>
	[JsonPropertyName("should_redemptions_skip_request_queue")]
	public bool ShouldRedemptionsSkipRequestQueue { get; set; }

	///<summary>
	///The number of redemptions redeemed during the current live stream.<br/>
	///The number counts against the max_per_stream_setting limit.<br/>
	///This field is null if the broadcaster’s stream isn’t live or max_per_stream_setting isn’t enabled.
	///</summary>
	[JsonPropertyName("redemptions_redeemed_current_stream")]
	public int RedemptionsRedeemedCurrentStream { get; set; }

	///<summary>
	///The timestamp of when the cooldown period expires.<br/>
	///Is null if the reward isn’t in a cooldown state (see the global_cooldown_setting field).<br/>
	///(Converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("cooldown_expires_at")]
	public DateTimeOffset CooldownExpiresAt { get; set; }
}



public class MaxPerStreamSetting
{
	///<summary>
	///A Boolean value that determines whether the reward applies a limit on the number of redemptions allowed per live stream. Is true if the reward applies a limit.
	///</summary>
	[JsonPropertyName("is_enabled")]
	public bool IsEnabled { get; set; }

	///<summary>
	///The maximum number of redemptions allowed per live stream.
	///</summary>
	[JsonPropertyName("max_per_stream")]
	public int MaxPerStream { get; set; }
}

public class MaxPerUserPerStreamSetting
{
	///<summary>
	///A Boolean value that determines whether the reward applies a limit on the number of redemptions allowed per user per live stream. Is true if the reward applies a limit.
	///</summary>
	[JsonPropertyName("is_enabled")]
	public bool IsEnabled { get; set; }

	///<summary>
	///The maximum number of redemptions allowed per user per live stream.
	///</summary>
	[JsonPropertyName("max_per_user_per_stream")]
	public int MaxPerUserPerStream { get; set; }
}

public class GlobalCooldownSetting
{
	///<summary>
	///A Boolean value that determines whether to apply a cooldown period. Is true if a cooldown period is enabled.
	///</summary>
	[JsonPropertyName("is_enabled")]
	public bool IsEnabled { get; set; }

	///<summary>
	///The cooldown period, in seconds.
	///</summary>
	[JsonPropertyName("global_cooldown_seconds")]
	public int GlobalCooldownSeconds { get; set; }
}