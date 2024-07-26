using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ GET == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-predictions">Twitch Documentation</see><br/>
	/// Gets a list of Channel Points Predictions that the broadcaster created.<br/>
	/// Required Scopes: channel:read:predictions or channel:manage:predictions<br/>
	/// </summary>
	public async Task GetPredictions(GetPredictionsRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "predictions";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetPredictionsRequestParameters
{
	///<summary>
	///The ID of the broadcaster whose predictions you want to get.<br/>
	///This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID of the prediction to get.<br/>
	///To specify more than one ID, include this parameter for each prediction you want to get.<br/>
	///For example, id=1234 id=5678. You may specify a maximum of 25 IDs.<br/>
	///The endpoint ignores duplicate IDs and those not owned by the broadcaster.
	///</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	///<summary>
	///The maximum number of items to return per page in the response.<br/>
	///The minimum page size is 1 item per page and the maximum is 100 items per page. The default is 20.
	///</summary>
	[JsonPropertyName("first")]
	public int First { get; set; }

	///<summary>
	///The cursor used to get the next page of results.<br/>
	///The Pagination object in the response contains the cursor’s value
	///</summary>
	[JsonPropertyName("after")]
	public string After { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class GetPredictionsResponse
{
	///<summary>
	///The broadcaster’s list of Channel Points Predictions. The list is sorted in descending ordered by when the prediction began (the most recent prediction is first). The list is empty if the broadcaster hasn’t created predictions.
	///</summary>
	[JsonPropertyName("data")]
	public List<PredictionData> Data { get; set; }


	///<summary>
	///The information used to page through the list of results.<br/>
	///The object is empty if there are no more pages left to page through. 
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }
}