using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.TwitchUsers.Extensions;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.TwitchUsers;

// Todo: Logic if attendance status is changed when stream is live.

public record TwitchUserRegistration(
	IAppCache Cache, KoishibotDbContext Database
	) : ITwitchUserRegistration
{
	public async Task<(TwitchUser user, bool notCached)> Start(TwitchUserDto userDto)
	{
		// Find user in Cache
		var cachedUser = Cache.FindUserByTwitchId(userDto.TwitchId);
		if (cachedUser is not null)
		{
			if (cachedUser.ChangedUsername(userDto.Name))
			{
				cachedUser.UpdateUserInfo(userDto);

				await Database.UpdateUser(cachedUser);
				Cache.UpdateUser(cachedUser);
			}

			return (cachedUser, false);
		}

		// Find user in Database
		var storedUser = await Database.GetUserByTwitchId(userDto.TwitchId);
		if (storedUser is null)
		{
			storedUser = new TwitchUser().Initialize(userDto);

			await Database.UpdateUser(storedUser);
			Cache.AddUser(storedUser);
		}
		else
		{
			if (storedUser.Permissions == PermissionLevel.Everyone)
			{
				storedUser.UpgradePermissions();
			}

			if (storedUser.ChangedUsername(userDto.Name))
			{
				storedUser.UpdateUserInfo(userDto);

				await Database.UpdateUser(storedUser);
				Cache.UpdateUser(storedUser);
			}
			else
			{
				Cache.AddUser(storedUser);
			}
		}

		return (storedUser, true);
	}
}

