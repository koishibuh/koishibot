using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.Application.Models;

public class ServiceStatus
{
	public string SignalR { get; set; } = ServiceStatusString.Offline;
	public string BotIrc { get; set; } = ServiceStatusString.Offline;
	public string TwitchWebsocket { get; set; } = ServiceStatusString.Offline;
	public string ObsWebsocket { get; set; } = ServiceStatusString.Offline;
	public string StreamElements { get; set; } = ServiceStatusString.Offline;

	public string StreamOnline { get; set; } = ServiceStatusString.Offline;
	public string Attendance { get; set; } = ServiceStatusString.Online;
	public string DragonEggQuest { get; set; } = ServiceStatusString.Offline;
	public string Dandle { get; set; } = ServiceStatusString.Offline;
	public string Raid { get; set; } = ServiceStatusString.Offline;
}