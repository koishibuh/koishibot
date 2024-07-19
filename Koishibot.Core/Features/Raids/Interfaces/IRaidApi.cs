using Koishibot.Core.Features.RaidSuggestions.Models;

namespace Koishibot.Core.Features.Raids.Interfaces;
public interface IRaidApi
{
	Task UpdateStreamTitle(string title);
	Task<List<FollowingLiveStreamInfo>?> GetLiveFollowedStreamers();
	Task<DateTime?> GetNextScheduledStreamDate();
	Task StartRaid(string raidingUserID);
	Task CancelRaid();
}