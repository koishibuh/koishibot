//using Koishibot.Core.Features.Common;
//using Koishibot.Core.Features.Common.Models;
//using Koishibot.Core.Features.RaidSuggestions.Interfaces;

//namespace Koishibot.Core.Features.RaidSuggestions;

//public partial record RaidSuggestionsApi(ITwitchAPI TwitchApi,
//	IOptions<Settings> Settings, IRefreshAccessTokenService TokenProcessor,
//	ILogger<SendAnnouncementApi> Log) : IRaidSuggestionsApi
//{
//	public string StreamerId => Settings.Value.StreamerTokens.UserId;

//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-users">Get Users Documentation</see> <br/>
//	/// BroadcasterId, BroadcasterLogin, BroadcasterName<br/>
//	/// Type (Admin, Global Mod, Staff, "" Normal User), Broadcaster Type (Affiliate, Partner, "" Normal)<br/>
//	/// ChannelDescription, ProPic Url, Offline Url, Created At<br/>
//	/// Up to 100 users in one request
//	/// </summary>
//	/// <returns></returns>
//	public async Task<UserInfo?> GetUserInfoByLogin(string username)
//	{
//		await TokenProcessor.EnsureValidToken();

//		var users = new List<string> { username };

//		var result = await TwitchApi.Helix.Users.GetUsersAsync(logins: users);
//		return result is null || result.Users.Length == 0
//			? null
//			: result.ConvertToDto();
//	}
//}