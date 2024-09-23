using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.Application.Models;

public class ServiceStatus
{
	public string SignalR { get; set; } = Status.Offline;
	public string BotIrc { get; set; } = Status.Offline;
	public string TwitchWebsocket { get; set; } = Status.Offline;
	public string ObsWebsocket { get; set; } = Status.Offline;
	public string StreamElements { get; set; } = Status.Offline;

	public string StreamOnline { get; set; } = Status.Offline;
	public string Attendance { get; set; } = Status.Online;
	public string DragonEggQuest { get; set; } = Status.Offline;
	public string Dandle { get; set; } = Status.Offline;
	public string Raid { get; set; } = Status.Offline;
}