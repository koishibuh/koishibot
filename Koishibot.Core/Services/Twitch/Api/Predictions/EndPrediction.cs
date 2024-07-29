using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#end-prediction">Twitch Documentation</see><br/>
	/// Locks, resolves, or cancels a Channel Points Prediction.<br/>
	/// Required Scopes: channel:manage:predictions<br/>
	/// </summary>
	public async Task EndPrediction(EndPredictionRequestBody requestBody)
	{
		var method = HttpMethod.Patch;
		var url = "predictions";
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		var response = await TwitchApiClient.SendRequest(method, url, body);
	}
}

// == ⚫ REQUEST BODY == //

public class EndPredictionRequestBody
{
	///<summary>
	///The ID of the broadcaster that’s running the prediction.<br/>
	///This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID of the prediction to update.
	///</summary>
	[JsonPropertyName("id")]
	public string PredictionId { get; set; }
	///<summary>
	///The status to set the prediction to.<br/>
	///The broadcaster can update an active prediction to LOCKED, RESOLVED, or CANCELED; and update a locked prediction to RESOLVED or CANCELED.<br/>
	///The broadcaster has up to 24 hours after the prediction window closes to resolve the prediction.<br/>
	///If not, Twitch sets the status to CANCELED and returns the points.
	///</summary>
	[JsonPropertyName("status")]
	[JsonConverter(typeof(PredictionApiStatusEnumConverter))]
	public PredictionApiStatus Status { get; set; }

	///<summary>
	///The ID of the winning outcome.<br/>
	///You must set this parameter if you set status to RESOLVED.
	///</summary>
	[JsonPropertyName("winning_outcome_id")]
	public string WinningOutcomeId { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class EndPredictionResponse
{
	///<summary>
	///A list that contains the single prediction that you updated.
	///</summary>
	[JsonPropertyName("data")]
	public List<PredictionData> Data { get; set; }
}