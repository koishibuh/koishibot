namespace Koishibot.Core.Features.Application.Models;

public class ServiceStatus
{
	public bool SignalR { get; set; } = false;
	public bool StreamerIrc { get; set; } = false;
	public bool BotIrc { get; set; } = false;
	public bool TwitchWebsocket { get; set; } = false;
	public bool ObsWebsocket { get; set; } = false;

	public bool StreamOnline { get; set; } = false;
	public bool Attendance { get; set; } = true;
	public bool DragonEggQuest { get; set; } = false;
	public bool Dandle { get; set; } = false;
	public bool Raid { get; set; } = false;	
}