﻿using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Converters;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-creator-goals">Twitch Documentation</see><br/>
	/// Gets the broadcaster’s list of active goals. Use this endpoint to get the current progress of each goal.<br/>
	/// Required Scopes: channel:read:goals<br/>
	/// </summary>
	public async Task GetCreatorGoals(GetCreatorGoalsRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "goals";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //


public class GetCreatorGoalsRequestParameters
{
	///<summary>
	///The ID of the broadcaster that created the goals.<br/>
	///This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class GetCreatorGoalsResponse
{
	///<summary>
	///The list of goals.</summary>br> The list is empty if the broadcaster hasn’t created goals.
	///</summary>
	[JsonPropertyName("data")]
	public List<CreatorGoalData> Data { get; set; }
}

public class CreatorGoalData
{
	///<summary>
	///An ID that identifies this goal.
	///</summary>
	[JsonPropertyName("id")]
	public string GoalId { get; set; }

	///<summary>
	///An ID that identifies the broadcaster that created the goal.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The broadcaster’s display name.
	///</summary>
	[JsonPropertyName("broadcaster_name")]
	public string BroadcasterUsername { get; set; }

	///<summary>
	///The broadcaster’s login name.
	///</summary>
	[JsonPropertyName("broadcaster_login")]
	public string Broadcaster { get; set; }

	///<summary>
	///The type of goal.
	///</summary>
	[JsonPropertyName("type")]
	[JsonConverter(typeof(GoalTypeEnumConverter))]
	public GoalType GoalType { get; set; }

	///<summary>
	///A description of the goal. Is an empty string if not specified.
	///</summary>
	[JsonPropertyName("description")]
	public string Description { get; set; }

	///<summary>
	///The goal’s current value.
	///</summary>
	[JsonPropertyName("current_amount")]
	public int CurrentAmount { get; set; }

	///<summary>
	///The goal’s target value.<br/>
	///For example, if the broadcaster has 200 followers before creating the goal, and their goal is to double that number, this field is set to 400.
	///</summary>
	[JsonPropertyName("target_amount")]
	public int TargetAmount { get; set; }

	///<summary>
	///The timestamp when the broadcaster created the goal.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("created_at")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset CreatedAt { get; set; }
}