namespace Koishibot.Core.Features.ChatCommands.Models;

public record ChatCommandDto(
	int Id,
	string Description,
	bool Enabled, 
	string Message,
	string Permissions,
	TimeSpan UserCooldown,
	DateTimeOffset? UserCooldownUp,
	TimeSpan GlobalCooldown,
	DateTimeOffset? GlobalCooldownUp)
{
	public DateTimeOffset? userCooldownUp { get; set; } = UserCooldownUp;
	public DateTimeOffset? globalCooldownUp { get; set; } = GlobalCooldownUp;
};