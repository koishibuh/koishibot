//using Koishibot.Core.Features.Common;
//using Koishibot.Core.Features.Raids.Enums;
//using Koishibot.Core.Features.RaidSuggestions.Extensions;
//using Koishibot.Core.Features.RaidSuggestions.Interfaces;
//using Koishibot.Core.Features.RaidSuggestions.Models;
//using Koishibot.Core.Features.StreamInformation.Models;
//using Koishibot.Core.Features.TwitchUsers.Models;
//namespace Koishibot.Core.Features.RaidSuggestions;

//public record RaidSuggestionValidation(IChatMessageService BotIrc,
//		IAppCache Cache, IRaidSuggestionsApi TwitchApi
//		) : IRaidSuggestionValidation
//{
//	public async Task Start(TwitchUser suggestedBy, string suggestedStreamer)
//	{
//		var details = (suggestedBy.Name, suggestedStreamer);

//		if (suggestedStreamer is "elysiagriffin")
//		{
//			await BotIrc.PostSuggestionResult(Code.CantSuggestMe, details);
//			return;
//		}

//		var raidSuggestions = Cache.GetRaidSuggestions();

//		if (raidSuggestions.StreamerAlreadySuggested(suggestedStreamer))
//		{
//			await BotIrc.PostSuggestionResult(Code.DupeSuggestion, details);
//			return;
//		}

//		var streamerInfo = await TwitchApi.GetUserInfoByLogin(suggestedStreamer);
//		if (streamerInfo is null)
//		{
//			await BotIrc.PostSuggestionResult(Code.NotValidUser, details);
//			return;
//		}

//		var liveStream = await TwitchApi.GetLiveStream(streamerInfo.TwitchId);
//		if (liveStream is null)
//		{
//			await BotIrc.PostSuggestionResult(Code.StreamerOffline, details);
//			return;
//		}

//		if (liveStream.StreamerOverMaxViewerCount(100))
//		{
//			await BotIrc.PostSuggestionResult(Code.MaxViewerCount, details);
//		}
//		else if (await TwitchApi.IsChatRestricted(liveStream.TwitchUserId))
//		{
//			await BotIrc.PostSuggestionResult(Code.ChatIsRestricted, details);
//		}
//		else
//		{
//			var suggestion = new RaidSuggestion()
//					.Set(suggestedBy, streamerInfo, liveStream);

//			raidSuggestions.Add(suggestion);
//			Cache.Add(raidSuggestions);

//			await BotIrc.PostSuggestionResult(Code.SuggestionSuccessful, details);
//		}
//	}
//}

//// == ⚫ TWITCH API == //

//public partial record RaidSuggestionsApi : IRaidSuggestionsApi
//{
//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-streams">Get Streams Documentation</see><br/>
//	/// Id, UserId, UserLogin, UserName, GameId, GameName <br/>
//	/// Type, Title, Tags, ViewerCount, StartedAt, Language<br/>
//	/// ThumbnailUrl, TagIds, IsMature<br/>
//	/// If stream is offline, streams will return null.
//	/// </summary>
//	/// <returns></returns>
//	public async Task<LiveStreamInfo?> GetLiveStream(string streamerId)
//	{
//		await TokenProcessor.EnsureValidToken();
//		var streamerIds = new List<string> { streamerId };

//		var result = await TwitchApi.Helix.Streams.GetStreamsAsync(first: 1, userIds: streamerIds);
//		if (result is null || result.Streams.Length == 0)
//		{
//			Log.LogError("Stream is offline");
//			return null;
//		}

//		return result.ConvertToDto();
//	}

//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-followed-channels">Get Followed Streams Documentation</see><br/>
//	/// Gets a list of broadcasters that I'm following that are currently live
//	/// </summary>
//	/// <returns></returns>
//	public async Task<List<FollowingLiveStreamInfo>> GetFollowedLiveStreams()
//	{
//		await TokenProcessor.EnsureValidToken();

//		var results = await TwitchApi.Helix.Streams.GetFollowedStreamsAsync(StreamerId);
//		return results is null || results.Data.Length == 0
//			? throw new Exception("Unable to get Followed Live Streams from Api")
//			: results.ConvertToDto();
//	}


//	/// <summary>
//	/// <see href="	https://dev.twitch.tv/docs/api/reference/#get-chat-settings">Get Chat Settings Documentation</see>
//	/// </summary>
//	/// <returns></returns>
//	public async Task<bool> IsChatRestricted(string userId)
//	{
//		await TokenProcessor.EnsureValidToken();

//		var result = await TwitchApi.Helix.Chat.GetChatSettingsAsync(userId, userId);

//		return result.Data.Length == 0
//			? true
//			: result.IsCheckRestricted();
//	}
//}