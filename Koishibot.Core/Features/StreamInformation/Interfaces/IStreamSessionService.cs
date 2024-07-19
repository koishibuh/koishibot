using Koishibot.Core.Features.StreamInformation.Models;
namespace Koishibot.Core.Features.StreamInformation.Interfaces;

public interface IStreamSessionService
{
	Task CreateOrReloadStreamSession();
	Task RecordNewSession(LiveStreamInfo streamInfo);
	Task ReloadCurrentSession(TwitchStream twitchStream);
}
