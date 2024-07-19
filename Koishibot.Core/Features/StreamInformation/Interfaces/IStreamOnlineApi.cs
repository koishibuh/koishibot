using Koishibot.Core.Features.StreamInformation.Models;

namespace Koishibot.Core.Features.StreamInformation.Interfaces;
public interface IStreamOnlineApi
{
	Task<LiveStreamInfo?> GetLiveStream(string streamerId);
}