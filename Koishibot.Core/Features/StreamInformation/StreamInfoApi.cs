using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.StreamInformation.Models;

namespace Koishibot.Core.Features.StreamInformation;

public partial record StreamInfoApi(
	ILogger<StreamInfoApi> Log,
	ITwitchAPI TwitchApi, 
	IOptions<Settings> Settings,
	IRefreshAccessTokenService TokenProcessor
	) : IStreamInfoApi
{
	public string StreamerId => Settings.Value.StreamerTokens.UserId;
}

public interface IStreamInfoApi
{
	Task<LiveStreamInfo?> GetLiveStream(string streamerId);
	Task<StreamInfo> GetStreamInfo(string streamerId);
	Task<RecentVod?> GetRecentVod(string userId);
}
