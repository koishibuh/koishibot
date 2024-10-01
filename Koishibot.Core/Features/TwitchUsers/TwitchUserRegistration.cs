using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.Common;
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
		if (cachedUser.InCache())
		{
			if (!cachedUser.ChangedUsername(userDto.Name))
				return (cachedUser, false);

			cachedUser.UpdateUserInfo(userDto);
			await Database.UpdateEntry(cachedUser);
			Cache.UpdateUser(cachedUser);

			return (cachedUser, false);
		}

		// Find user in Database
		var storedUser = await Database.GetUserByTwitchId(userDto.TwitchId);
		if (storedUser.NotInDatabase())
		{
			storedUser = new TwitchUser().Initialize(userDto);

			await Database.UpdateUser(storedUser);
			Cache.AddUser(storedUser);
		}
		else
		{
			if (storedUser!.HasEveryonePermissions())
			{
				storedUser.UpgradePermissions();
				await Database.UpdateUser(storedUser);
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