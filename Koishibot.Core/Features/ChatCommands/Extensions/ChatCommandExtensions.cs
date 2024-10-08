﻿using Koishibot.Core.Features.ChatCommands.Controllers;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;
namespace Koishibot.Core.Features.ChatCommands.Extensions;
public static class ChatCommandExtensions
{
	public static async Task<int> UpdateEntry<T>
	(this KoishibotDbContext database, T entity) where T : class, IEntity
	{
		database.Update(entity);
		await database.SaveChangesAsync();
		return entity.Id;
	}

	public static async Task RemoveEntry<T>
		(this KoishibotDbContext database, T entity) where T : class, IEntity
	{
		database.Remove(entity);
		await database.SaveChangesAsync();
	}

	public static async Task<T> UpdateEntryReturn<T>
(this KoishibotDbContext database, T entity) where T : class, IEntity
	{
		database.Update(entity);
		await database.SaveChangesAsync();
		return entity;
	}

	public static ChatCommandDto? GetCommand(this IAppCache cache, string commandName, string permissions)
	{
		var result = cache.Get<Dictionary<string, ChatCommandDto>>(CacheName.Commands);
		if (result is null) { return null; }

		//return result.Where(x => x.Key == commandName).FirstOrDefault().Value;
		var item = result.FirstOrDefault(x => x.Key == commandName && x.Value?.Permissions == permissions);
		return item.Value ?? null;
	}

	public static async Task<Dictionary<string, ChatCommandDto>> GetCommand(this KoishibotDbContext database, string command)
	{
		var commandNames = await database.CommandNames
			.Include(x => x.ChatCommand)
			.Where(x => x.Name == command)
			.Select(x => x.ChatCommand)
			.ToListAsync();

		return commandNames.Count == 0
			? null
			: commandNames.ToDictionary(
				item => command,
				item => new ChatCommandDto(
					item.Id, item.Description,
					item.Enabled, item.Message,
					item.Permissions, item.UserCooldown,
					DateTimeOffset.UtcNow, item.GlobalCooldown,
					DateTimeOffset.UtcNow)
				);
	}

	public static void AddCommand(this IAppCache cache, Dictionary<string, ChatCommandDto> command)
	{
		var result = cache.Get<Dictionary<string, ChatCommandDto>>(CacheName.Commands);
		if (result is null) 
		{
			cache.Add(CacheName.Commands, command, TimeSpan.FromHours(1));
		}
		else
		{
			foreach(var item in command)
			{
				result.TryAdd(item.Key, item.Value);
			}
		}
	}
}

public interface IEntity
{
	int Id { get; set; }
}