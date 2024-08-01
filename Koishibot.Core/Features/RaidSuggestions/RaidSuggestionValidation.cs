using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.RaidSuggestions.Enums;
using Koishibot.Core.Features.RaidSuggestions.Extensions;
using Koishibot.Core.Features.RaidSuggestions.Interfaces;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.RaidSuggestions;

public record RaidSuggestionValidation(
	IOptions<Settings> Settings,
	IAppCache Cache,
	ITwitchApiRequest TwitchApiRequest,
	IChatReplyService ChatReplyService
		) : IRaidSuggestionValidation
{
	public string StreamerId => Settings.Value.StreamerTokens.UserId;

	public async Task Start(TwitchUser suggestedBy, string suggestedStreamer)
	{
		var data = new UserStreamerData(suggestedBy.Name, suggestedStreamer);


		if (suggestedStreamer is "elysiagriffin")
		{
			await ChatReplyService.App(Command.CantSuggestMe, data);
			return;
		}

		var raidSuggestions = Cache.GetRaidSuggestions();

		if (raidSuggestions.StreamerAlreadySuggested(suggestedStreamer))
		{
			await ChatReplyService.App(Command.DupeSuggestion, data);
			return;
		}

		var userParameters = new GetUsersRequestParameters
		{
			UserIds = new List<string> { suggestedStreamer }
		};
		var streamerInfo = await TwitchApiRequest.GetUsers(userParameters);

		if (streamerInfo is null)
		{
			await ChatReplyService.App(Command.NotValidUser, data);
			return;
		}

		var liveStreamParameters = new GetLiveStreamsRequestParameters
		{
			UserIds = new List<string> { streamerInfo[0].Id },
			First = 1
		};
		var liveStreamResponse = await TwitchApiRequest.GetLiveStreams(liveStreamParameters);
		if (liveStreamResponse.Data.Count == 0)
		{
			await ChatReplyService.App(Command.StreamerOffline, data);
			return;
		}

		var liveStream = liveStreamResponse.Data[0].ConvertToDto();
		if (liveStream.StreamerOverMaxViewerCount(100))
		{
			await ChatReplyService.App(Command.MaxViewerCount, data);
			return;
		}

		var settingsParameters = new GetChatSettingsRequestParameters
		{
			BroadcasterId = liveStream.TwitchUserId
		};

		var chatSettingsResponse = await TwitchApiRequest.GetChatSettings(settingsParameters);

		if (chatSettingsResponse.FollowerMode == true || chatSettingsResponse.SubscriberMode == true)
		{
			await ChatReplyService.App(Command.RestrictedChat, data);
			return;
		}

		var suggestion = new RaidSuggestion()
				.Set(suggestedBy, streamerInfo[0].CreateDto(), liveStream);

		raidSuggestions.Add(suggestion);
		Cache.Add(raidSuggestions);

		await ChatReplyService.App(Command.SuggestionSuccessful, data);
	}
}