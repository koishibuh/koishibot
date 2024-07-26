//using Koishibot.Core.Features.Raids.Enums;
//using Koishibot.Core.Features.RaidSuggestions.Extensions;
//using Koishibot.Core.Features.RaidSuggestions.Interfaces;
//using Koishibot.Core.Features.RaidSuggestions.Models;
//using Koishibot.Core.Features.TwitchUsers.Extensions;
//namespace Koishibot.Core.Features.RaidSuggestions;

//public record SelectRaidCandidatesService(IAppCache Cache,
//		IChatMessageService BotIrc, IRaidSuggestionsApi TwitchApi,
//		ISignalrService Signalr
//		) : ISelectRaidCandidatesService
//{
//	public async Task Start()
//	{
//		Cache.UpdateStatusToVoting();
//		await BotIrc.RaidStatus(Code.SuggestionsClosed);

//		var suggestions = Cache.GetRaidSuggestions();
//		if (suggestions.Count is 0)
//		{
//			suggestions = await GetSuggestionsFromFollowList(5);
//			Cache.AddRaidSuggestions(suggestions);
//		}
//		else if (suggestions.Count < 3)
//		{
//			var result = await GetSuggestionsFromFollowList(3);
//			suggestions.AddRange(result);

//			Cache.AddRaidSuggestions(suggestions);
//		}

//		var selectedCandidates = suggestions.Select3RandomCandidates();

//		suggestions.RemoveRaidCandidates(selectedCandidates);
//		Cache.AddRaidCandidatesAndSuggestions(suggestions, selectedCandidates);

//		var viewModel = selectedCandidates.ConvertToVm();
//		await Signalr.SendRaidCandidates(viewModel);
//	}

//	// == ⚫  == //

//	public async Task<List<RaidSuggestion>> GetSuggestionsFromFollowList(int number)
//	{
//		var results = await TwitchApi.GetFollowedLiveStreams()
//										?? throw new Exception("Unable to get followed list from Api");

//		var random = new Random();
//		var selectedSuggestions = results
//						.OrderBy(r => random.Next())
//						.Where(r => r.LiveStreamInfo.ViewerCount < 100)
//						.Take(number)
//						.ToList();

//		var raidCandidates = new List<RaidSuggestion>();

//		var me = Cache.FindUserByTwitchId("98683749");
//		foreach (var streamer in selectedSuggestions)
//		{
//			var userInfo = await TwitchApi.GetUserInfoByLogin(streamer.User.Name);
//			if (userInfo is null) { continue; }

//			raidCandidates.Add(new RaidSuggestion
//			{
//				SuggestedByUser = me!,
//				Streamer = userInfo,
//				StreamInfo = streamer.LiveStreamInfo
//			});
//		}

//		return raidCandidates;
//	}
//}