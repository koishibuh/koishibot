using Koishibot.Core.Features.Application.Models;
using Koishibot.Core.Features.ChatCommands.Controllers;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Persistence.Cache.Enums;
using Microsoft.Extensions.Caching.Memory;

namespace Koishibot.Core.Persistence.Cache;

public record AppCache(
IMemoryCache Cache, ISignalrService Signalr,
IServiceScopeFactory ScopeFactory
) : IAppCache
{
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