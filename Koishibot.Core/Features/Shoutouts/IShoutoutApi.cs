using Koishibot.Core.Features.Common.Models;

namespace Koishibot.Core.Features.Shoutouts;
public interface IShoutoutApi
{
	Task<string?> GetChannelTrailer(string userId);
	Task<RecentVod?> GetRecentVod(string userId);
	Task<string?> GetRecentVodId(string userId);
	Task<StreamInfo> GetStreamInfo(string streamerId);
	Task<string?> GetTopClip(string userId);
	Task SendShoutout(string userId);
}