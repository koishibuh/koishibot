using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ POST == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#create-custom-rewards">Twitch Documentation</see><br/>
	/// Creates a Custom Reward in the broadcaster’s channel.<br/>
	/// The maximum number of custom rewards per channel is 50, which includes both enabled and disabled rewards.
	/// Required Scopes: channel:manage:redemptions
	/// </summary>
	public async Task CreateCustomReward
		(CreateCustomRewardRequestParameters parameters, CreateCustomRewardRequestBody requestBody)
	{
		var method = HttpMethod.Post;
		var url = "channel_points/custom_rewards";
		var query = parameters.ObjectQueryFormatter();
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		var response = await TwitchApiClient.SendRequest(method, url, query, body);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class CreateCustomRewardRequestParameters
{
	///<summary>
	///The ID of the broadcaster to add the custom reward to.<br/>
	///This ID must match the user ID found in the OAuth token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

}

// == ⚫ REQUEST BODY == //

/// <summary>
/// <see href="https://dev.twitch.tv/docs/api/reference/#create-custom-rewards">Twitch Documentation</see><br/>
/// Creates a Custom Reward in the broadcaster’s channel.<br/>
/// The maximum number of custom rewards per channel is 50, which includes both enabled and disabled rewards.<br/>
/// Required Scopes: channel:manage:redemptions<br/>
/// </summary>
public class CreateCustomRewardRequestBody
{
	///<summary>
	///The custom reward’s title.<br/>
	///The title may contain a maximum of 45 characters and it must be unique amongst all of the broadcaster’s custom rewards.<br/>
	///REQUIRED
	///</summary>
	[JsonPropertyName("title")]
	public string Title { get; set; } = string.Empty;

	///<summary>
	///The cost of the reward, in Channel Points. The minimum is 1 point.<br/>
	///REQUIRED
	///</summary>
	[JsonPropertyName("cost")]
	public int Cost { get; set; }

	///<summary>
	///The prompt shown to the viewer when they redeem the reward.<br/>
	///Specify a prompt if is_user_input_required is true.<br/>
	///The prompt is limited to a maximum of 200 characters.
	///</summary>
	[JsonPropertyName("prompt")]
	public string? Prompt { get; set; }

	///<summary>
	///A Boolean value that determines whether the reward is enabled.<br/>
	///Viewers see only enabled rewards.<br/>
	///The default is true.
	///</summary>
	[JsonPropertyName("is_enabled")]
	public bool IsEnabled { get; set; }

	///<summary>
	///The background color to use for the reward.<br/>
	///Specify the color using Hex format (for example, #9147FF).
	///</summary>
	[JsonPropertyName("background_color")]
	public string? BackgroundColor { get; set; }

	///<summary>
	///A Boolean value that determines whether the user needs to enter information when redeeming the reward.<br/>
	///See the prompt field.<br/>
	///The default is false.
	///</summary>
	[JsonPropertyName("is_user_input_required")]
	public bool IsUserInputRequired { get; set; }

	///<summary>
	///A Boolean value that determines whether to limit the maximum number of redemptions allowed per live stream (see the max_per_stream field). The default is false.
	///</summary>
	[JsonPropertyName("is_max_per_stream_enabled")]
	public bool IsMaxPerStreamEnabled { get; set; }

	///<summary>
	///The maximum number of redemptions allowed per live stream.<br/>
	///Applied only if is_max_per_stream_enabled is true.<br/>
	///The minimum value is 1.
	///</summary>
	[JsonPropertyName("max_per_stream")]
	public int MaxPerStream { get; set; }

	///<summary>
	///A Boolean value that determines whether to limit the maximum number of redemptions allowed per user per stream (see the max_per_user_per_stream field).<br/>
	///The default is false.
	///</summary>
	[JsonPropertyName("is_max_per_user_per_stream_enabled")]
	public bool IsMaxPerUserPerStreamEnabled { get; set; }

	///<summary>
	///The maximum number of redemptions allowed per user per stream.<br/>
	///Applied only if is_max_per_user_per_stream_enabled is true.<br/>
	///The minimum value is 1.
	///</summary>
	[JsonPropertyName("max_per_user_per_stream")]
	public int MaxPerUserPerStream { get; set; }

	///<summary>
	///A Boolean value that determines whether to apply a cooldown period between redemptions (see the global_cooldown_seconds field for the duration of the cooldown period).<br/>
	///The default is false.
	///</summary>
	[JsonPropertyName("is_global_cooldown_enabled")]
	public bool IsGlobalCooldownEnabled { get; set; }

	///<summary>
	///The cooldown period, in seconds.<br/>
	///Applied only if the is_global_cooldown_enabled field is true.<br/>
	///The minimum value is 1; however, the minimum value is 60 for it to be shown in the Twitch UX.
	///</summary>
	[JsonPropertyName("global_cooldown_seconds")]
	public int GlobalCooldownSeconds { get; set; }

	///<summary>
	///A Boolean value that determines whether redemptions should be set to FULFILLED status immediately when a reward is redeemed.<br/>
	///If false, status is set to UNFULFILLED and follows the normal request queue process.<br/>
	///The default is false.
	///</summary>
	[JsonPropertyName("should_redemptions_skip_request_queue")]
	public bool ShouldRedemptionsSkipRequestQueue { get; set; }
}


// == ⚫ RESPONSE BODY == //

public class CreateCustomRewardResponse
{
	///<summary>
	///A list that contains the single custom reward you created.
	///</summary>
	[JsonPropertyName("data")]
	public List<CustomRewardData> CustomRewardData { get; set; }
}
