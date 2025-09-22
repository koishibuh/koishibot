using Koishibot.Core.Features.Application.Models;
using Koishibot.Core.Features.ChatCommands.Controllers;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Persistence.Cache.Enums;
using Microsoft.Extensions.Caching.Memory;

namespace Koishibot.Core.Persistence.Cache;

public record AppCache(
IMemoryCache Cache, ISignalrService Signalr,
IServiceScopeFactory ScopeFactory
) : IAppCache
{
	
	public T GetOrCreate<T>(CacheName name, Func<ICacheEntry, T> factory)
{
    return Cache.GetOrCreate(name, factory);
}
	public void Add<T>(CacheName name, T item)
	{
		Cache.Set(name, item, TimeSpan.FromHours(6));
	}

	public void Add<T>(CacheName name, T item, TimeSpan expiresAt)
	{
		Cache.Set(name, item, expiresAt);
	}

	public void AddNoExpire<T>(CacheName name, T item)
	{
		Cache.Set(name, item);
	}

	public T? Get<T>(CacheName name) where T : class
	{
		var value = Cache.Get<T>(name);
		return value as T;
	}

	public void Remove(CacheName name) => Cache.Remove(name);

	// == ⚫ == //

	public void InitializeServiceStatusCache()
	{
		var serviceStatus = new ServiceStatus();
		Cache.Set(CacheName.ServiceStatus, serviceStatus);
	}

	public async Task LoadCommandCache()
	{
		using var scope = ScopeFactory.CreateScope();
		var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();
		var databaseResult = await database.GetAllCommands();
		
		Cache.Set(CacheName.Commands, databaseResult);
	}
	
	public async Task LoadNewCommandCache()
	{
		using var scope = ScopeFactory.CreateScope();
		var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();
		var command = await database.NewGetAllCommands();
		Cache.Set(CacheName.ChatCommands, command);
		var databaseResult = await database.GetAllChatResponses();
		Cache.Set(CacheName.ChatResponses, databaseResult);
	}


	public async Task LoadRecentSessionCache()
	{
		using var scope = ScopeFactory.CreateScope();
		var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();
		var databaseResult = await database.GetRecentStreamSession();
		var lastMandatorySessionId = await database.GetLastMandatorySessionId();

		var cacheSession = new CurrentSession
		{
			LiveStreamId = databaseResult.LiveStreams[0]?.Id, // check this
			StreamSessionId = databaseResult.Id,
			LastMandatorySessionId = lastMandatorySessionId,
			Summary = databaseResult.Summary,
		};

		Cache.Set(CacheName.CurrentSession, cacheSession);
	}

	public async Task UpdateCommandCache()
	{
		Remove(CacheName.Commands);
		await LoadCommandCache();
	}
	

	public async Task UpdateServiceStatus(ServiceName name, string status)
	{
		var serviceStatus = Cache.Get<ServiceStatus>(CacheName.ServiceStatus)
			?? throw new Exception("Service Status not found");

		var result = typeof(ServiceStatus).GetProperty(name.ToString());
		if (result is not null)
		{
			result.SetValue(serviceStatus, status);
		}
		else
		{
			throw new Exception("Property not found");
		}

		Cache.Set(CacheName.ServiceStatus, serviceStatus);

		var statusVm = new ServiceStatusVm(name.ToString(), status);
		await Signalr.SendStatusUpdate(statusVm);
	}
}