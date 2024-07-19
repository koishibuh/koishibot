using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Features.StreamInformation.Models;

namespace Koishibot.Core.Features.RaidSuggestions.Interfaces;
public interface IRaidSuggestionsApi
{
	Task UpdateStreamTitle(string title);
	Task<LiveStreamInfo?> GetLiveStream(string streamerId);
	Task<List<FollowingLiveStreamInfo>> GetFollowedLiveStreams();
	Task<bool> IsChatRestricted(string userId);
	Task<UserInfo?> GetUserInfoByLogin(string username);
	Task<string?> StartPoll(string title, List<string> choiceList, int duration);
	Task AnnounceRaidCandidateOptions(List<RaidSuggestion> raidCandidates);
}