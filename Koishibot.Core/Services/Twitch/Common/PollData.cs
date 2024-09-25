using Koishibot.Core.Services.Twitch.Converters;
using Koishibot.Core.Services.Twitch.Enums;
namespace Koishibot.Core.Services.Twitch.Common;

public class PollData
{
	///<summary>
	///An ID that identifies the poll.
	///</summary>
	[JsonPropertyName("id")]
	public string PollId { get; set; }

	///<summary>
	///An ID that identifies the broadcaster that created the poll.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The broadcaster's display name.
	///</summary>
	[JsonPropertyName("broadcaster_name")]
	public string BroadcasterName { get; set; }

	///<summary>
	///The broadcaster's login name.
	///</summary>
	[JsonPropertyName("broadcaster_login")]
	public string BroadcasterLogin { get; set; }

	///<summary>
	///The question that viewers are voting on.<br/>
	///For example, What game should I play next?<br/>
	///The title may contain a maximum of 60 characters.
	///</summary>
	[JsonPropertyName("title")]
	public string PollTitle { get; set; }

	///<summary>
	///A list of choices that viewers can choose from.<br/>
	///The list will contain a minimum of two choices and up to a maximum of five choices.
	///</summary>
	[JsonPropertyName("choices")]
	public List<Choice> Choices { get; set; }

	/////<summary>
	/////Not used; will be set to false.
	/////</summary>
	//[JsonPropertyName("bits_voting_enabled")]
	//public bool BitsVotingEnabled { get; set; }

	/////<summary>
	/////Not used; will be set to 0.
	/////</summary>
	//[JsonPropertyName("bits_per_vote")]
	//public int BitsPerVote { get; set; }

	///<summary>
	///A Boolean value that indicates whether viewers may cast additional votes using Channel Points.
	///</summary>
	[JsonPropertyName("channel_points_voting_enabled")]
	public bool ChannelPointsVotingEnabled { get; set; }

	///<summary>
	///The number of points the viewer must spend to cast one additional vote.
	///</summary>
	[JsonPropertyName("channel_points_per_vote")]
	public int ChannelPointsPerVote { get; set; }

	///<summary>
	///The poll's status.
	///</summary>
	[JsonPropertyName("status")]
	[JsonConverter(typeof(PollStatusEnumConverter))]
	public PollStatus Status { get; set; }

	///<summary>
	///The length of time (in seconds) that the poll will run for.
	///</summary>
	[JsonPropertyName("duration")]
	public int DurationInSeconds { get; set; }

	///<summary>
	///The timestamp of when the poll began.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("started_at")]
	public DateTimeOffset StartedAt { get; set; }

	///<summary>
	///The timestamp of when the poll ended.<br/>
	///If status is ACTIVE, this field is set to null.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("ended_at")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset? EndedAt { get; set; }
}

public class Choice
{
	///<summary>
	///An ID that identifies this choice.
	///</summary>
	[JsonPropertyName("id")]
	public string ChoiceId { get; set; } = null!;

	///<summary>
	///The choice's title.<br/>
	///The title may contain a maximum of 25 characters.
	///</summary>
	[JsonPropertyName("title")]
	public string Title { get; set; } = null!;

	///<summary>
	///The total number of votes cast for this choice.
	///</summary>
	[JsonPropertyName("votes")]
	public int Votes { get; set; }

	///<summary>
	///The number of votes cast using Channel Points.
	///</summary>
	[JsonPropertyName("channel_points_votes")]
	public int ChannelPointsVotes { get; set; }

	/////<summary>
	/////Not used; will be set to 0.
	/////</summary>
	//[JsonPropertyName("bits_votes")]
	//public int BitsVotes { get; set; }
}