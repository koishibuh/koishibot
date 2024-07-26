using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;


public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#create-prediction">Twitch Documentation</see><br/>
	/// Creates a Channel Points Prediction.<br/>
	/// Required Scopes: channel:manage:predictions<br/>
	/// </summary>
	public async Task CreatePrediction(CreatePredictionRequestBody requestBody)
	{
		var method = HttpMethod.Post;
		var url = "predictions";
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		var response = await TwitchApiClient.SendRequest(method, url, body);
	}
}

// == ⚫ REQUEST BODY == //

public class CreatePredictionRequestBody
{
	///<summary>
	///The ID of the broadcaster that’s running the prediction. <br/>
	///This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The question that the broadcaster is asking.<br/>
	///For example, Will I finish this entire pizza?<br/>
	///The title is limited to a maximum of 45 characters.
	///</summary>
	[JsonPropertyName("title")]
	public string PredictionTitle { get; set; }

	///<summary>
	///The list of possible outcomes that the viewers may choose from.<br/>
	///The list must contain a minimum of 2 choices and up to a maximum of 10 choices.
	///</summary>
	[JsonPropertyName("outcomes")]
	public List<OutcomeTitle> Outcomes { get; set; }

	///<summary>
	///The length of time (in seconds) that the prediction will run for.<br/>
	///The minimum is 30 seconds and the maximum is 1800 seconds (30 minutes).
	///</summary>
	[JsonPropertyName("prediction_window")]
	public int PredictionWindowInSeconds { get; set; }
}

public class OutcomeTitle
{
	///<summary>
	///The text of one of the outcomes that the viewer may select.<br/>
	///The title is limited to a maximum of 25 characters.
	///</summary>
	[JsonPropertyName("title")]
	public string Title { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class CreatePredictionResponse
{
	///<summary>
	///A list that contains the single prediction that you created.
	///</summary>
	[JsonPropertyName("data")]
	public List<PredictionData> Data { get; set; }
}