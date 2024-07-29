using Koishibot.Core.Features.Raids.Enums;
using Koishibot.Core.Features.RaidSuggestions.Extensions;
using Koishibot.Core.Features.RaidSuggestions.Interfaces;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.RaidSuggestions;

public record RaidSuggestionValidation(
	IOptions<Settings> Settings,
	ITwitchIrcService BotIrc,
	IAppCache Cache, 
	ITwitchApiRequest TwitchApiRequest
		) : IRaidSuggestionValidation
{
	public string StreamerId => Settings.Value.StreamerTokens.UserId;

	public async Task Start(TwitchUser suggestedBy, string suggestedStreamer)
	{
		var details = (suggestedBy.Name, suggestedStreamer);

		if (suggestedStreamer is "elysiagriffin")
		{
			await BotIrc.PostSuggestionResult(Code.CantSuggestMe, details);
			return;
		}

		var raidSuggestions = Cache.GetRaidSuggestions();

		if (raidSuggestions.StreamerAlreadySuggested(suggestedStreamer))
		{
			await BotIrc.PostSuggestionResult(Code.DupeSuggestion, details);
			return;
		}

		var userParameters = new GetUsersRequestParameters
		{
			UserIds = new List<string> { suggestedStreamer }
		};
		var streamerInfo = await TwitchApiRequest.GetUsers(userParameters);

		if (streamerInfo is null)
		{
			await BotIrc.PostSuggestionResult(Code.NotValidUser, details);
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
			await BotIrc.PostSuggestionResult(Code.StreamerOffline, details);
			return;
		}

		var liveStream = liveStreamResponse.Data[0].ConvertToDto();
		if (liveStream.StreamerOverMaxViewerCount(100))
		{
			await BotIrc.PostSuggestionResult(Code.MaxViewerCount, details);
			return;
		}

		var settingsParameters = new GetChatSettingsRequestParameters
		{
			BroadcasterId = liveStream.TwitchUserId
		};

		var chatSettingsResponse = await TwitchApiRequest.GetChatSettings(settingsParameters);

		if (chatSettingsResponse.FollowerMode == true || chatSettingsResponse.SubscriberMode == true)
		{
			await BotIrc.PostSuggestionResult(Code.ChatIsRestricted, details);
			return;
		}
		
			var suggestion = new RaidSuggestion()
					.Set(suggestedBy, streamerInfo[0].CreateDto(), liveStream);

			raidSuggestions.Add(suggestion);
			Cache.Add(raidSuggestions);

			await BotIrc.PostSuggestionResult(Code.SuggestionSuccessful, details);
		
	}
}