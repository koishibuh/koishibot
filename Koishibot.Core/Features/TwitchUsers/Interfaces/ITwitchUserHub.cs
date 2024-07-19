using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.TwitchUsers.Interfaces;

public interface ITwitchUserHub
{
	Task<TwitchUser> Start(TwitchUserDto userDto);
}