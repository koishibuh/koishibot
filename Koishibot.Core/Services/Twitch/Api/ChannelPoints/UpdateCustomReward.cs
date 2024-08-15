using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
namespace Koishibot.Core.Services.TwitchApi.Models;

/*════════════════【 API REQUEST 】════════════════*/
public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-custom-reward">Twitch Documentation</see><br/>
	/// Updates a custom reward. The app used to create the reward is the only app that may update the reward.<br/>
	/// Required Scopes: channel:manage:redemptions<br/>
	/// </summary>
	public async Task UpdateCustomReward
		(UpdateCustomRewardRequestParameters parameters, UpdateCustomRewardRequestBody requestBody)
	{
		var method = HttpMethod.Patch;
		const string url = "channel_points/custom_rewards";
		var query = parameters.ObjectQueryFormatter();
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		await TwitchApiClient.SendRequest(method, url, query, body);
	}
}

/*═════════════【 REQUEST PARAMETERS 】═════════════*/
public class UpdateCustomRewardRequestParameters
{
	///<summary>
	///The ID of the broadcaster updating the reward.<br/>
	///This ID must match the user ID found in the OAuth token.<br/>
	///REQUIRED
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; } = null!;

	///<summary>
	///The ID of the reward to update.<br/>
	///REQUIRED
	///</summary>
	[JsonPropertyName("id")]
	public string RewardId { get; set; } = null!;
}

/*════════════════【 REQUEST BODY 】════════════════*/
public class UpdateCustomRewardRequestBody
{
	///<summary>
	///The reward’s title.<br/>
	///The title may contain a maximum of 45 characters and it must be unique amongst all of the broadcaster’s custom rewards.
	///</summary>
	[JsonPropertyName("title")]
	public string? Title { get; set; }

	///<summary>
	///The prompt shown to the viewer when they redeem the reward.<br/>
	///Specify a prompt if is_user_input_required is true.<br/>
	///The prompt is limited to a maximum of 200 characters.
	///</summary>
	[JsonPropertyName("prompt")]
	public string? Prompt { get; set; }

	///<summary>
	///The cost of the reward, in channel points. The minimum is 1 point.
	///</summary>
	[JsonPropertyName("cost")]
	public int? Cost { get; set; }

	///<summary>
	///The background color to use for the reward.<br/>
	///Specify the color using Hex format (for example, #00E5CB).
	///</summary>
	[JsonPropertyName("background_color")]
	public string? BackgroundColor { get; set; }

	///<summary>
	///A Boolean value that indicates whether the reward is enabled.<br/>
	///Set to true to enable the reward. Viewers see only enabled rewards.
	///</summary>
	[JsonPropertyName("is_enabled")]
	public bool? IsEnabled { get; set; }

	///<summary>
	///A Boolean value that determines whether users must enter information to redeem the reward. Set to true if user input is required.<br/>
	///See the prompt field.
	///</summary>
	[JsonPropertyName("is_user_input_required")]
	public bool? IsUserInputRequired { get; set; }

	///<summary>
	///A Boolean value that determines whether to limit the maximum number of redemptions allowed per live stream (see the max_per_stream field).<br/>
	///Set to true to limit redemptions.
	///</summary>
	[JsonPropertyName("is_max_per_stream_enabled")]
	public bool? IsMaxPerStreamEnabled { get; set; }

	///<summary>
	///The maximum number of redemptions allowed per live stream.<br/>
	///Applied only if is_max_per_stream_enabled is true. The minimum value is 1.
	///</summary>
	[JsonPropertyName("max_per_stream")]
	public int? MaxPerStream { get; set; }

	///<summary>
	///A Boolean value that determines whether to limit the maximum number of redemptions allowed per user per stream (see max_per_user_per_stream).<br/>
	///The minimum value is 1. Set to true to limit redemptions.
	///</summary>
	[JsonPropertyName("is_max_per_user_per_stream_enabled")]
	public bool? IsMaxPerUserPerStreamEnabled { get; set; }

	///<summary>
	///The maximum number of redemptions allowed per user per stream.<br/>
	///Applied only if is_max_per_user_per_stream_enabled is true.
	///</summary>
	[JsonPropertyName("max_per_user_per_stream")]
	public int? MaxPerUserPerStream { get; set; }

	///<summary>
	///A Boolean value that determines whether to apply a cooldown period between redemptions.<br/>
	///Set to true to apply a cooldown period. For the duration of the cooldown period, see global_cooldown_seconds.
	///</summary>
	[JsonPropertyName("is_global_cooldown_enabled")]
	public bool? IsGlobalCooldownEnabled { get; set; }

	///<summary>
	///The cooldown period, in seconds. Applied only if is_global_cooldown_enabled is true.<br/>
	///The minimum value is 1; however, for it to be shown in the Twitch UX, the minimum value is 60.
	///</summary>
	[JsonPropertyName("global_cooldown_seconds")]
	public int? GlobalCooldownSeconds { get; set; }

	///<summary>
	///A Boolean value that determines whether to pause the reward.<br/>
	///Set to true to pause the reward. Viewers can’t redeem paused rewards..
	///</summary>
	[JsonPropertyName("is_paused")]
	public bool? IsPaused { get; set; }

	///<summary>
	///A Boolean value that determines whether redemptions should be set to FULFILLED status immediately when a reward is redeemed.<br/>
	///If false, status is set to UNFULFILLED and follows the normal request queue process.
	///</summary>
	[JsonPropertyName("should_redemptions_skip_request_queue")]
	public bool? ShouldRedemptionsSkipRequestQueue { get; set; }
}

/*══════════════════【 RESPONSE 】══════════════════*/
public class UpdateCustomRewardResponse
{
	///<summary>
	///The list contains the single reward that you updated.
	///</summary>
	[JsonPropertyName("data")]
	public List<CustomRewardData>? CustomRewardData { get; set; }
}