using Koishibot.Core.Features.Common.Models;

namespace Koishibot.Core.Features.StreamInformation.Interfaces;
public interface IGetStreamInfoApi
{
	Task<StreamInfo> GetStreamInfo(string streamerId);
}
