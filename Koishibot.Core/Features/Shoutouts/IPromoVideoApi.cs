using Koishibot.Core.Features.Common.Models;

namespace Koishibot.Core.Features.Shoutouts;
public interface IPromoVideoApi
{
	Task<string?> GetChannelTrailer(string userId);
	Task<string?> GetRecentVodId(string userId);
	Task<string?> GetTopClip(string userId);
}