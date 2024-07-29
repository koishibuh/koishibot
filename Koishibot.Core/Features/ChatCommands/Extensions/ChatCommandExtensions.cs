using Koishibot.Core.Features.ChatCommands.Controllers;
using Koishibot.Core.Persistence;

namespace Koishibot.Core.Features.ChatCommands.Extensions;
public static class ChatCommandExtensions
{
	public static async Task<bool> IsTimerGroupNameUnique
		(this CreateTimerGroupCommand command, KoishibotDbContext database)
	{
		var result = await database.TimerGroups
			.FirstOrDefaultAsync(p => p.Name == command.Name);

		return result is null;
	}

	public static async Task<int> UpdateEntry<T>
	(this KoishibotDbContext database, T entity) where T: class, IEntity
	{
		database.Update(entity);
		await database.SaveChangesAsync();
		return entity.Id;
	}

	public static async Task<T> UpdateEntryReturn<T>
(this KoishibotDbContext database, T entity) where T : class, IEntity
	{
		database.Update(entity);
		await database.SaveChangesAsync();
		return entity;
	}


	public static async Task<bool> IsCommandNameUnique
	(this CreateCommandNameCommand command, KoishibotDbContext database)
	{
		var result = await database.CommandNames
			.FirstOrDefaultAsync(p => p.Name == command.Name);

		return result is null;
	}
}

public interface IEntity
{
	int Id { get; set; }
}