using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.TwitchUsers.Extensions;

public static class TwitchUserExtension
{
	// CACHE

	public static TwitchUser? FindUserByTwitchId(this IAppCache cache, string id)
	{
		var userList = cache.Get<List<TwitchUser>>(CacheName.Users)
		               ?? new List<TwitchUser>();

		// userList ??= new List<TwitchUser>();
		return userList.Find(x => x.TwitchId == id);
	}

	public static void AddUser(this IAppCache cache, TwitchUser user)
	{
		var userList = cache.Get<List<TwitchUser>>(CacheName.Users)
		               ?? new List<TwitchUser>();

		userList.Add(user);
		cache.Add(CacheName.Users, userList, TimeSpan.FromDays(1));
	}

	public static void UpdateUser(this IAppCache cache, TwitchUser user)
	{
		var userList = cache.Get<List<TwitchUser>>(CacheName.Users)
		               ?? new List<TwitchUser>();

		var index = userList!.FindIndex(x => x.TwitchId == user.TwitchId);

		if (index != -1)
		{
			userList[index] = user;
		}

		cache.Add(CacheName.Users, cache, TimeSpan.FromDays(1));
	}

	// DATABASE

	public static async Task<TwitchUser?> GetUserByTwitchId
	(this KoishibotDbContext Context, string twitchId)
	{
		return await Context.Users
		.FirstOrDefaultAsync(tu => tu.TwitchId == twitchId);
	}

	public static async Task<TwitchUser?> GetUserByLogin
	(this KoishibotDbContext context, string userlogin)
	{
		return await context.Users
		.FirstOrDefaultAsync(x => x.Login == userlogin);
	}

	public static async Task<TwitchUser> UpdateUser
	(this KoishibotDbContext database, TwitchUser user)
	{
		database.Update(user);
		await database.SaveChangesAsync();
		return user;
	}
}