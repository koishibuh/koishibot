using Koishibot.Core.Features.StreamInformation.Models;
namespace Koishibot.Core.Features.StreamInformation.Interfaces;

public interface IStreamSessionService
{
	Task CreateOrReloadStreamSession();
	Task RecordNewSession(LiveStream stream);
	Task ReloadCurrentSession(LiveStream stream);
}