using Koishibot.Core.Features.ChatCommands.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.ChatCommands.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class ChatCommand : IEntity
{
	public int Id { get; set; }

	public string Category { get; set; } = CommandCategory.Misc;
	public string Description { get; set; } = string.Empty;
	public bool Enabled { get; set; }
	public string Message { get; set; } = null!;
	public string Permissions { get; set; } = PermissionLevel.Everyone;
	public TimeSpan UserCooldown { get; set; }
	public TimeSpan GlobalCooldown { get; set; }

	public List<CommandName> CommandNames { get; set; } = [];
	public List<TimerGroup>? TimerGroups { get; set; } = [];
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class ChatCommandConfig : IEntityTypeConfiguration<ChatCommand>
{
	public void Configure(EntityTypeBuilder<ChatCommand> builder)
	{
		builder.ToTable("ChatCommands");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.Category);
		builder.Property(p => p.Description);
		builder.Property(p => p.Enabled);

		builder
		.Property(p => p.Message)
		.HasMaxLength(500);

		builder.Property(p => p.Permissions);
		builder.Property(p => p.UserCooldown);
		builder.Property(p => p.GlobalCooldown);

		builder
		.HasMany(p => p.TimerGroups)
		.WithMany(p => p.Commands)
		.UsingEntity<CommandTimerGroup>();

		builder
		.HasMany(p => p.CommandNames)
		.WithOne(p => p.ChatCommand)
		.HasForeignKey(p => p.ChatCommandId)
		.IsRequired(false);
	}
}