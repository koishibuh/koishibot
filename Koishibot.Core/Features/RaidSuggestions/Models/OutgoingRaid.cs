using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.RaidSuggestions.Models;

public class OutgoingRaid
{
	public int Id { get; set; }
	public int TwitchStreamId { get; set; }
	public int RaidedUserId { get; set; }
	public int SuggestedByUserId { get; set; }

	// NAVIGATION 

	public TwitchStream TwitchStream { get; set; } = null!;
	public TwitchUser RaidedUser { get; set; } = null!;
	public TwitchUser SuggestedByUser { get; set; } = null!;

	public OutgoingRaid Set(int streamId, int raidedUserId, int suggestedByUserId)
	{
		TwitchStreamId = streamId;
		RaidedUserId = raidedUserId;
		SuggestedByUserId = suggestedByUserId;
		return this;
	}

}
