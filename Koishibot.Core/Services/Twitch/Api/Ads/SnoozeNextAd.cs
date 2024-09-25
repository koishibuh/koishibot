using Koishibot.Core.Features.AdBreak.Controllers;
using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Converters;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;

/*════════════════【 API REQUEST 】════════════════*/
public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#snooze-next-ad">Twitch Documentation</see><br/>
	/// If available, pushes back the timestamp of the upcoming automatic mid-roll ad by 5 minutes.<br/>
	/// This endpoint duplicates the snooze functionality in the creator dashboard’s Ads Manager.<br/>
	/// Required Scopes: channel:manage:ads
	/// </summary>
	public async Task<SnoozeNextAdResponse> SnoozeNextAd(SnoozeNextAdRequestParameters parameters)
	{
		var method = HttpMethod.Post;
		const string url = "ads/schedule/snooze";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);

		var result = JsonSerializer.Deserialize<SnoozeNextAdResponse>(response);
		return result ?? throw new Exception("Failed to deserialize response");
	}
}

/*════════════════【 REQUEST BODY 】════════════════*/
public class SnoozeNextAdRequestParameters
{
	///<summary>
	/// Provided broadcaster_id must match the user_id in the auth<br/>
	/// REQUIRED
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; } = null!;
}

/*══════════════════【 RESPONSE 】══════════════════*/
///<summary>
///<see href="https://dev.twitch.tv/docs/api/reference/#snooze-next-ad">Twitch Documentation</see><br/>
///Info about the channel’s snoozes and next upcoming ad after successfully snoozing.
///</summary>
public class SnoozeNextAdResponse
{
	///<summary>
	///The number of snoozes available for the broadcaster.
	///</summary>
	[JsonPropertyName("snooze_count")]
	public int AvailableSnoozeCount { get; set; }

	///<summary>
	///The timestamp when the broadcaster will gain an additional snooze.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("snooze_refresh_at")]
	public DateTimeOffset GainNextSnoozeAt { get; set; }

	///<summary>
	///The timestamp of the broadcaster’s next scheduled ad.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("next_ad_at")]
	public DateTimeOffset NextAdScheduledAt { get; set; }

	public SnoozeNextAdDto ConvertToDto()
	{
		return new SnoozeNextAdDto(
			AvailableSnoozeCount,
			GainNextSnoozeAt,
			NextAdScheduledAt
			);
	}
}