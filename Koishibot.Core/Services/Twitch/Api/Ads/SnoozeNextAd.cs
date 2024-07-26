using Koishibot.Core.Features.AdBreak.Controllers;
using Koishibot.Core.Services.Twitch.EventSubs.Converters;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ POST == //

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
		var url = "ads/schedule/snooze";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);

		var result = JsonSerializer.Deserialize<SnoozeNextAdResponse>(response);
		return result is not null
			? result
			: throw new Exception("Failed to deserialize response");
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class SnoozeNextAdRequestParameters
{
	///<summary>
	/// Provided broadcaster_id must match the user_id in the auth<br/>
	/// REQUIRED
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; } = null!;
}

// == ⚫ RESPONSE BODY == //

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
	[JsonConverter(typeof(DateTimeOffsetConverter))]
	public DateTimeOffset GainNextSnoozeAt { get; set; }

	///<summary>
	///The timestamp of the broadcaster’s next scheduled ad.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("next_ad_at")]
	[JsonConverter(typeof(DateTimeOffsetConverter))]
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