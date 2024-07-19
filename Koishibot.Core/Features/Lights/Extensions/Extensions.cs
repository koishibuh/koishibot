using Koishibot.Core.Features.Lights.Models;
using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.Lights.Extensions;

public static class Extensions
{
	public static void AddLights(this IAppCache cache, List<Light> lights)
	{
		cache.Add(CacheName.LedLights, lights);
	}

	public static void RemoveLights(this IAppCache cache)
	{
		cache.Remove(CacheName.LedLights);
	}

	public static List<Light>? GetLights(this IAppCache cache)
	{
		return cache.Get<List<Light>>(CacheName.LedLights);
	}

	//

	public static DataCommandItem CreateDataCommand(this List<Light> lights, 
		string command, string macAddress)
	{
		return new DataCommandItem
		{
			hexData = command,
			macAddress = macAddress
		};
	}


}
