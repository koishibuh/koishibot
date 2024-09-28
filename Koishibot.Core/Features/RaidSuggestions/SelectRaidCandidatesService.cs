using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.RaidSuggestions.Enums;
using Koishibot.Core.Features.RaidSuggestions.Extensions;
using Koishibot.Core.Features.RaidSuggestions.Interfaces;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.RaidSuggestions;

/*═══════════════════【 SERVICE 】═══════════════════*/
public record SelectRaidCandidatesService(
IOptions<Settings> Settings,
IAppCache Cache,
ITwitchApiRequest TwitchApiRequest,
IChatReplyService ChatReplyService,
ISignalrService Signalr
) : ISelectRaidCandidatesService
{
	public async Task Start()
	{
		Cache.UpdateStatusToVoting();
		await ChatReplyService.App(Command.VotingSoon);

		var suggestions = Cache.GetRaidSuggestions();
		switch (suggestions.Count)
		{
			case 0:
				suggestions = await GetSuggestionsFromFollowList(5);
				Cache.AddRaidSuggestions(suggestions);
				break;
			case < 3:
			{
				var result = await GetSuggestionsFromFollowList(3);
				suggestions.AddRange(result);

				Cache.AddRaidSuggestions(suggestions);
				break;
			}
		}

		var selectedCandidates = suggestions.Select3RandomCandidates();

		suggestions.RemoveRaidCandidates(selectedCandidates);
		Cache.AddRaidCandidatesAndSuggestions(suggestions, selectedCandidates);

		var viewModel = selectedCandidates.ConvertToVm();
		await Signalr.SendRaidCandidates(viewModel);
	}

	// == ⚫  == //
	private async Task<List<RaidSuggestion>> GetSuggestionsFromFollowList(int number)
	{

		var parameters = new GetFollowedLiveStreamsRequestParameters
		{
			UserId = Settings.Value.StreamerTokens.UserId
		};

		var results = await TwitchApiRequest.GetFollowedLiveStreams(parameters);
		
		var random = new Random();
		var selectedSuggestions = results.Data
			.OrderBy(r => random.Next())
			.Where(r => r.ViewerCount < 100)
			.Take(number)
			.ToList();

		var raidCandidates = new List<RaidSuggestion>();

		var me = Cache.FindUserByTwitchId("98683749");

		var userParameters = new GetUsersRequestParameters
		{
			UserLogins = selectedSuggestions.Select(x => x.BroadcasterLogin).ToList()
		};

		var result1 = await TwitchApiRequest.GetUsers(userParameters);

		var raidSuggestionList = new List<RaidSuggestion>();

		foreach (var streamer1 in selectedSuggestions)
		{
			var x = result1.Where(x => x.Id == streamer1.BroadcasterId).FirstOrDefault();

			var aSuggestion = new RaidSuggestion
			{
				SuggestedByUser = me!,
				Streamer = new UserInfo(x.Id, x.Login, x.Name, x.BroadcasterType.ToString(), x.ChannelDescription, x.ProfileImageUrl),
				StreamInfo = new LiveStreamInfo(streamer1.BroadcasterId, streamer1.StreamId, streamer1.CategoryId, streamer1.CategoryName, streamer1.StreamTitle, streamer1.ViewerCount, streamer1.StartedAt, streamer1.ThumbnailUrl)
			};

			raidSuggestionList.Add(aSuggestion);
		}

		return raidCandidates;
	}
}