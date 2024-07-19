using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.Raids.Interfaces;

public interface IPromoVideoService
{
	Task<string?> Start(TwitchUser user);
}