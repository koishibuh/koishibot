using Koishibot.Core.Features.Application.Models;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.Common;

public static class AppCacheExtensions
{
	public static void AddStreamEvent(this IAppCache cache, StreamEventVm streamEventVm)
	{
		var streamEvents = cache.Get<List<StreamEventVm>>(CacheName.StreamEvents);
		streamEvents ??= new List<StreamEventVm>();

		streamEvents.Add(streamEventVm);
		cache.Add(CacheName.StreamEvents, streamEvents);
	}

	public static bool GetStatusByServiceName(this IAppCache cache, ServiceName serviceName)
	{
		var serviceStatus = cache.Get<ServiceStatus>(CacheName.ServiceStatus)
			?? throw new Exception("Status not found");

		var result = typeof(ServiceStatus).GetProperty(serviceName.ToString());

		return result is not null
		? (bool)result.GetValue(serviceStatus)!
		: throw new Exception($"Property '{serviceName.ToString()}' not found in ServiceStatus");


		//var result = serviceStatus.serviceName;
		//return result is null
		//? false
		//: result.Status;
	}

	public static int GetCurrentStreamId(this IAppCache cache)
	{
		var streamSessions = cache.Get<StreamSessions>(CacheName.StreamSessions);
		if (streamSessions is null) { return 0; }
		return streamSessions.CurrentStream.Id;
	}

	public static TwitchStream GetCurrentTwitchStream(this IAppCache cache)
	{
		var streamSessions = cache.Get<StreamSessions>(CacheName.StreamSessions);
		return streamSessions!.CurrentStream;
	}



	public static StreamInfo? GetStreamInfo(this IAppCache cache)
	{
		return cache.Get<StreamInfo>(CacheName.StreamInfo);
	}

}