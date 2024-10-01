using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
namespace Koishibot.Core.Services.TwitchApi.Models;

/*══════════════════【 CONTROLLER 】══════════════════*/
public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#create-poll">Twitch Documentation</see><br/>
	/// Creates a poll that viewers in the broadcaster’s channel can vote on.<br/>
	/// The poll begins as soon as it’s created. You may run only one poll at a time.<br/>
	/// Required Scopes: channel:manage:polls<br/>
	/// </summary>
	public async Task CreatePoll(CreatePollRequestBody requestBody)
	{
		var method = HttpMethod.Post;
		const string url = "polls";
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		await TwitchApiClient.SendRequest(method, url, body);
	}
}

/*════════════════【 REQUEST BODY 】════════════════*/
public class CreatePollRequestBody
{
	///<summary>
	///The ID of the broadcaster that’s running the poll.<br/>
	///This ID must match the user ID in the user access token.<br/>
	///REQUIRED
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The question that viewers will vote on.<br/>
	///For example, What game should I play next?<br/>
	///The question may contain a maximum of 60 characters.<br/>
	///REQUIRED
	///</summary>
	[JsonPropertyName("title")]
	public string PollTitle { get; set; }

	///<summary>
	///A list of choices that viewers may choose from.<br/>
	///The list must contain a minimum of 2 choices and up to a maximum of 5 choices.<br/>
	///REQUIRED
	///</summary>
	[JsonPropertyName("choices")]
	public List<ChoiceTitle> Choices { get; set; }

	///<summary>
	///The length of time (in seconds) that the poll will run for.<br/>
	///The minimum is 15 seconds and the maximum is 1800 seconds (30 minutes).<br/>
	///REQUIRED
	///</summary>
	[JsonPropertyName("duration")]
	public int DurationInSeconds { get; set; }

	///<summary>
	///A Boolean value that indicates whether viewers may cast additional votes using Channel Points.<br/>
	///If true, the viewer may cast more than one vote but each additional vote costs the number of Channel Points specified in channel_points_per_vote.<br/>
	///The default is false (viewers may cast only one vote).
	///</summary>
	[JsonPropertyName("channel_points_voting_enabled")]
	public bool ChannelPointsVotingEnabled { get; set; }

	///<summary>
	///The number of points that the viewer must spend to cast one additional vote.<br/>
	///The minimum is 1 and the maximum is 1000000. <br/>
	///Set only if ChannelPointsVotingEnabled is true.
	///</summary>
	[JsonPropertyName("channel_points_per_vote")]
	public int ChannelPointsPerVote { get; set; }
}

public class ChoiceTitle
{
	///<summary>
	///One of the choices the viewer may select.<br/>
	///The choice may contain a maximum of 25 characters.
	///</summary>
	[JsonPropertyName("title")]
	public string Title { get; set; }
}

/*══════════════════【 RESPONSE 】══════════════════*/
public class CreatePolLResponse
{
	///<summary>
	///A list that contains the single poll that you created.
	///</summary>
	[JsonPropertyName("data")]
	public List<PollData> Data { get; set; }
}