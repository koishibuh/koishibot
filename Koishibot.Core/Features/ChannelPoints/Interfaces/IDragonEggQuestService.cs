using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.ChannelPoints.Interfaces;

/*═══════════════════【 INTERFACE 】═══════════════════*/
public interface IDragonEggQuestService
{
	Task Initialize();
	Task GetResult(TwitchUser user, DateTimeOffset redeemedAt);
	Task<List<string>> GetEggDescriptions(string url);
}