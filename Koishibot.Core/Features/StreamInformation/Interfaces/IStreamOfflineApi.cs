using Koishibot.Core.Features.Common.Models;

namespace Koishibot.Core.Features.StreamInformation.Interfaces;

public interface IStreamOfflineApi
{
	Task<RecentVod?> GetRecentVod(string userId);
}
