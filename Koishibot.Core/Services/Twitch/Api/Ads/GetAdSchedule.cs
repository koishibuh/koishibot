using Koishibot.Core.Features.AdBreak.Models;
using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Converters;
using System.Text.Json;
namespace Koishibot.Core.Services.TwitchApi.Models;

/*════════════════【 API REQUEST 】════════════════*/
/// <summary>
/// <see href="https://dev.twitch.tv/docs/api/reference/#get-ad-schedule">Twitch Documentation</see><br/>
/// Returns ad schedule related information, including snooze, when the last ad was run, when the next ad is scheduled, and if the channel is currently in pre-roll free time.<br/>
/// A new ad cannot be run until 8 minutes after running a previous ad.<br/>
/// Required Scopes: channel:read:ads<br/>
/// </summary>
public partial record TwitchApiRequest : ITwitchApiRequest
{
	public async Task<AdScheduleData> GetAdSchedule(GetAdScheduleRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		const string url = "channels/ads";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);

		var result = JsonSerializer.Deserialize<GetAdScheduleResponse>(response)
			?? throw new Exception("Failed to deserialize response");

		// TODO: Only do this when stream is live?

		var retryCount = 0;
		while (result.Data[0].NextAdTimeStillValid() is false && retryCount < 3)
		{
			var delay = TimeSpan.FromSeconds(Math.Pow(4, retryCount));
			await Task.Delay(delay);
			retryCount++;

			response = await TwitchApiClient.SendRequest(method, url, query);
			result = JsonSerializer.Deserialize<GetAdScheduleResponse>(response)
				?? throw new Exception("Failed to deserialize response");
		}

		return result.Data[0];
	}
}

/*═════════════【 REQUEST PARAMETERS 】═════════════*/
public class GetAdScheduleRequestParameters
{
	/// <summary>
	/// Provided broadcaster_id must match the user_id in the auth token.<br/>
	/// REQUIRED
	/// </summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; } = null!;
}

/*══════════════════【 RESPONSE 】══════════════════*/
/// <summary>
/// <see href="https://dev.twitch.tv/docs/api/reference/#get-ad-schedule">Twitch Documentation</see><br/>
/// Information related to the channel’s ad schedule.
/// </summary>
public class GetAdScheduleResponse
{
	[JsonPropertyName("data")]
	public List<AdScheduleData> Data { get; set; }
}

public class AdScheduleData
{
	///<summary>
	///The number of snoozes available for the broadcaster.
	///</summary>
	[JsonPropertyName("snooze_count")]
	public int AvailableSnoozeCount { get; set; }

	///<summary>
	///The timestamp when the broadcaster will gain an additional snooze.<br/>
	///Only valid while stream is live, Twitch returns 0 if stream is offline.
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("snooze_refresh_at")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset? GainNextSnoozeAt { get; set; }

	///<summary>
	///The timestamp of the broadcaster’s next scheduled ad.<br/>
	///Twitch returns 0 if channel has no ad scheduled or is not live.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("next_ad_at")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset? NextAdScheduledAt { get; set; }

	///<summary>
	///The length in seconds of the scheduled upcoming ad break.
	///(Int converted to TimeSpan)
	///</summary>
	[JsonPropertyName("duration")]
	[JsonConverter(typeof(TimeSpanSecondsConverter))]
	public TimeSpan AdDuration { get; set; }

	///<summary>
	///The UTC timestamp of the broadcaster’s last ad-break.<br/>
	///Twitch returns 0 if the channel has not run an ad or is not live.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("last_ad_at")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset? LastAdPlayedAt { get; set; }

	///<summary>
	///The amount of pre-roll free time remaining for the channel in seconds.<br/>
	///Returns 0 if they are currently not pre-roll free.
	///</summary>
	[JsonPropertyName("preroll_free_time")]
	public int RemainingPrerollFreeTimeInSeconds { get; set; }

	public AdScheduleDto ConvertToDto() => new(
		AvailableSnoozeCount,
		GainNextSnoozeAt,
		NextAdScheduledAt,
		AdDuration,
		LastAdPlayedAt,
		RemainingPrerollFreeTimeInSeconds);

	public bool NextAdTimeStillValid() =>
		DateTimeOffset.UtcNow < NextAdScheduledAt;
}