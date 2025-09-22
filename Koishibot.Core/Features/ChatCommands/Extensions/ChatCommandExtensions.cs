using Koishibot.Core.Features.ChatCommands.Controllers;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;
using Microsoft.Extensions.Caching.Memory;
namespace Koishibot.Core.Features.ChatCommands.Extensions;
public static class ChatCommandExtensions
{
	public static async Task<int> AddEntry<T>
		(this KoishibotDbContext database, T entity) where T : class, IEntity
	{
		await database.Set<T>().AddAsync(entity);
		// database.Add(entity);
		try
		{
			await database.SaveChangesAsync();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	
		return entity.Id;
	}
	
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
	
	public static IAppCache CreateCommandCache(this IAppCache cache)
	{
		cache.AddNoExpire(CacheName.ChatCommands, new List<NewChatCommandDto>());
		cache.AddNoExpire(CacheName.ChatResponses, new List<ChatResponseDto>());
		return cache;
	}

	public static ChatCommandDto? GetCommand(this IAppCache cache, string commandName, string permissions)
	{
		var result = cache.Get<Dictionary<string, ChatCommandDto>>(CacheName.Commands);
		if (result is null) { return null; }

		//return result.Where(x => x.Key == commandName).FirstOrDefault().Value;
		var item = result.FirstOrDefault(x => x.Key == commandName && x.Value?.Permissions == permissions);
		return item.Value ?? null;
	}

	public static NewChatCommandDto? NewGetCommand(this IAppCache cache, string commandName, string permissions)
	{
		var result = cache.Get<List<NewChatCommandDto>>(CacheName.ChatCommands);
		if (result is null) { return null; }
		
		var item = result.FirstOrDefault(x => x.Name == commandName && x.Permissions == permissions);
		return item ?? null;
	}

	public static ChatResponseDto? GetResponse(this IAppCache cache, string commandName)
	{
		var result = cache.Get<List<ChatResponseDto>>(CacheName.ChatResponses);
		if (result is null) { return null; }
		
		var item = result.FirstOrDefault(x => x.InternalName == commandName);
		return item ?? null;
	}
	
	public static ChatResponseDto? GetResponseById(this IAppCache cache, int? CommandId)
	{
		var result = cache.Get<List<ChatResponseDto>>(CacheName.ChatResponses);
		if (result is null) { return null; }
		
		var item = result.FirstOrDefault(x => x.Id == CommandId);
		return item ?? null;
	}
	
	// public static async Task<Dictionary<string, ChatCommandDto>> GetCommand(this KoishibotDbContext database, string command)
	// {
	// 	var commandNames = await database.CommandNames
	// 		.Include(x => x.ChatCommand)
	// 		.Where(x => x.Name == command)
	// 		.Select(x => x.ChatCommand)
	// 		.ToListAsync();
	//
	// 	return commandNames.Count == 0
	// 		? null
	// 		: commandNames.ToDictionary(
	// 			item => command,
	// 			item => new ChatCommandDto(
	// 				item.Id, "", item.Category, item.Description,
	// 				item.Enabled, item.Message,
	// 				item.Permissions, item.UserCooldown,
	// 				DateTimeOffset.UtcNow, item.GlobalCooldown,
	// 				DateTimeOffset.UtcNow)
	// 			);
	// }
	//
	// public static async Task<Dictionary<string, ChatCommandDto>> GetAllCommands(this KoishibotDbContext database)
	// {
	// 	var result = await database.CommandNames
	// 		.Include(x => x.ChatCommand)
	// 		.ToListAsync();
	// 		
	// 	return result.ToDictionary(x => x.Name, x => new ChatCommandDto(
	// 		x.ChatCommand.Id, "", x.ChatCommand.Category, x.ChatCommand.Description,
	// 		x.ChatCommand.Enabled, x.ChatCommand.Message,
	// 		x.ChatCommand.Permissions, x.ChatCommand.UserCooldown,
	// 		DateTimeOffset.UtcNow, x.ChatCommand.GlobalCooldown,
	// 		DateTimeOffset.UtcNow));
	// }
	//
	public static async Task<List<NewChatCommandDto>> NewGetAllCommands(this KoishibotDbContext database)
	{
	return await database.ChatCommands
			.Select(x => new NewChatCommandDto(
				x.Id, x.Name, x.Category, x.Enabled, x.PermissionLevel, 
				x.UserCooldown, DateTimeOffset.UtcNow, x.GlobalCooldown, DateTimeOffset.UtcNow, 
				x.ResponseId))
			.ToListAsync();
	}
	
	public static async Task<List<ChatResponseDto>> GetAllChatResponses(this KoishibotDbContext database)
	{
		return await database.ChatResponses
			.Select(x => new ChatResponseDto(
				x.Id, x.InternalName, x.Description, x.Message))
			.ToListAsync();
	}
	
	public static async Task<ChatResponseDto?> GetResponseByName(this KoishibotDbContext database, string name)
	{
		return await database.ChatResponses
			.Where(x => x.InternalName == name)
			.Select(x => new ChatResponseDto(
				x.Id, x.InternalName, x.Description, x.Message))
			.FirstOrDefaultAsync();
	}

	public static void AddCommand(this IAppCache cache, Dictionary<string, ChatCommandDto> command)
	{
		var result = cache.Get<Dictionary<string, ChatCommandDto>>(CacheName.Commands);
		if (result is null) 
		{
			cache.AddNoExpire(CacheName.Commands, command);
		}
		else
		{
			foreach(var item in command)
			{
				result.TryAdd(item.Key, item.Value);
			}
		}
	}
	
	// NEW
	public static void AddResponse(this IAppCache cache, ChatResponseDto dto)
	{
		var responses = cache.GetOrCreate(CacheName.ChatResponses, entry =>
		{
			entry.SetSlidingExpiration(TimeSpan.FromHours(6));
			return new List<ChatResponseDto>();
		});

		lock (responses)
		{
			responses.Add(dto);
		}
	}
}

public interface IEntity
{
	int Id { get; set; }
}