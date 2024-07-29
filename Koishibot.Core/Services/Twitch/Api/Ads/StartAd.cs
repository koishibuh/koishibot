using Koishibot.Core.Features.AdBreak.Controllers;
using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json;
namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ POST == //

/// <summary>
/// <see href="https://dev.twitch.tv/docs/api/reference/#start-commercial">Twitch Documentation</see>
/// Starts a commercial on the specified channel.<br/>
/// NOTE: Only partners and affiliates may run commercials and they must be streaming live at the time.<br/>
/// NOTE: Only the broadcaster may start a commercial;<br/>
/// The broadcaster’s editors and moderators may not start commercials on behalf of the broadcaster.
/// </summary>
public partial record TwitchApiRequest : ITwitchApiRequest
{
	public async Task<StartAdResponse> StartAd(StartAdRequestBody requestBody)
	{
		var method = HttpMethod.Post;
		var url = "channels/commercial";
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		var response = await TwitchApiClient.SendRequest(method, url, body);

		var result = JsonSerializer.Deserialize<StartAdResponse>(response);
		return result is not null
			? result
			: throw new Exception("Failed to deserialize response");
	}
}

// == ⚫ REQUEST BODY == //

public class StartAdRequestBody
{
	/// <summary>
	/// The ID of the partner or affiliate broadcaster that wants to run the commercial.<br/>
	/// This ID must match the user ID found in the OAuth token.<br/>
	/// REQUIRED 
	/// </summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	/// <summary>
	/// The length of the commercial to run, in seconds.<br/>
	/// Twitch tries to serve a commercial that’s the requested length, but it may be shorter or longer.<br/>
	/// The maximum length you should request is 180 seconds. (3 minutes)
	/// </summary>
	[JsonPropertyName("length")]
	[JsonConverter(typeof(AdLengthEnumConverter))]
	public AdLength AdLength { get; set; }
}

// == ⚫ RESPONSE BODY == //

/// <summary>
/// <see href="https://dev.twitch.tv/docs/api/reference/#start-commercial">Twitch Documentation</see>
/// The status of your start commercial request.
/// </summary>
public class StartAdResponse
{
	///<summary>
	///The length of the commercial you requested.<br/>
	///If you request a commercial that’s longer than 180 seconds, the API uses 180 seconds.
	///</summary>
	[JsonPropertyName("length")]
	public int AdLength { get; set; }

	///<summary>
	///A message that indicates whether Twitch was able to serve an ad.
	///Example shows empty if succesful.
	///</summary>
	[JsonPropertyName("message")]
	public string Message { get; set; }

	///<summary>
	///The number of seconds you must wait before running another commercial.
	///</summary>
	[JsonPropertyName("retry_after")]
	public int AdCooldownSeconds { get; set; }

	public StartAdDto ConvertToDto()
	{
		return new StartAdDto(
			AdLength,
			Message,
			AdCooldownSeconds);
	}
}