using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.TwitchUsers.Interfaces;

public interface ITwitchUserRegistration
{
	Task<(TwitchUser user, bool notCached)> Start(TwitchUserDto userDto);
};