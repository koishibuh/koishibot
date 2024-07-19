using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.RaidSuggestions.Models;

public class RaidSuggestion
{
	public TwitchUser SuggestedByUser { get; set; } = null!;
	public UserInfo Streamer { get; set; } = null!;
	public LiveStreamInfo StreamInfo { get; set; } = null!;

	public RaidSuggestion Set(TwitchUser suggestedBy, UserInfo streamerInfo, LiveStreamInfo streamInfo)
	{
		SuggestedByUser = suggestedBy;
		Streamer = streamerInfo;
		StreamInfo = streamInfo;
		return this;
	}
};