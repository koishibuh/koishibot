//using Koishibot.Core.Features.Polls.Models;

//namespace Koishibot.Core.Features.Polls;

//public partial record TwitchPollApi(
//	ILogger<TwitchPollApi> Log,
//	ITwitchAPI TwitchApi, 
//	IRefreshAccessTokenService TokenProcessor,
//	IOptions<Settings> Settings
//	) : ITwitchPollApi
//{
//	public string StreamerId => Settings.Value.StreamerTokens.UserId;
//}

//public interface ITwitchPollApi
//{
//	Task<string?> StartPoll(PendingPoll pendingPoll);
//	Task<bool> EndPoll(string pollId);

//}