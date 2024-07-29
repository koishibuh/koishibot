using Koishibot.Core.Features.ChatCommands.Extensions;

namespace Koishibot.Core.Features.ChatCommands.Models;
public class ChatCommand : IEntity
{
	public int Id { get; set; }
	public string Description { get; set; } = string.Empty;
	public bool Enabled { get; set; }
	public string Message { get; set; } = null!;
	public PermissionLevel Permissions { get; set; }
	public TimeSpan UserCooldown { get; set; }
	public TimeSpan GlobalCooldown { get; set; }

	public List<CommandName> CommandNames { get; set; } = [];
	public List<TimerGroup> TimerGroups { get; set; } = [];
}

public enum PermissionLevel
{
	App = 1,
	Broadcaster = 2,
	Mod = 3,
	Vip = 4,
	Everyone = 5
}

public class TimerGroup : IEntity
{
	public int Id { get; set; }
	public string Name { get; set; } = null!;
	public string? Description { get; set; }
	public TimeSpan Interval { get; set; }

	public List<ChatCommand> Commands { get; } = [];
}

public class CommandTimerGroup
{
	public int ChatCommandId { get; set; }
	public int TimerGroupId { get; set; }
}

public class CommandName : IEntity
{
	public int Id { get; set; }
	public string Name { get; set; } = null!;
	public int? ChatCommandId { get; set; }
	public ChatCommand? ChatCommand { get; set; }
}