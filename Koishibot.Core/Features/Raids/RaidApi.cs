//using Koishibot.Core.Features.Common;
//using Koishibot.Core.Features.Raids.Interfaces;
//using Koishibot.Core.Features.RaidSuggestions.Models;

//namespace Koishibot.Core.Features.Raids;

//public partial record RaidApi(
//	ITwitchAPI TwitchApi,
//	IOptions<Settings> Settings,
//	IRefreshAccessTokenService TokenProcessor
//	) : IRaidApi
//{
//	public string StreamerId => Settings.Value.StreamerTokens.UserId;

//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-followed-streams">Get Followed Streams Documentation</see>
//	/// </summary>
//	/// <returns></returns>
//	public async Task<List<FollowingLiveStreamInfo>?> GetLiveFollowedStreamers()
//	{
//		await TokenProcessor.EnsureValidToken();

//		var results = await TwitchApi.Helix.Streams.GetFollowedStreamsAsync(StreamerId);
//		return results is null || results.Data.Length == 0
//			? null
//			: results.ConvertToDto();
//	}
//}