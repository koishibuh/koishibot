using Koishibot.Core.Features.ChatCommands.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Koishibot.Core.Persistence.Configurations;

public record ChatCommandConfig()
{
	public void Configure(EntityTypeBuilder<ChatCommand> builder)
	{
		builder.ToTable("ChatCommands");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);
		
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

public record CommandNameConfig()
{
	public void Configure(EntityTypeBuilder<CommandName> builder)
	{
		builder.ToTable("CommandNames");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder
			.HasIndex(p => p.Name)
			.IsUnique();

		builder.Property(p => p.Name);
	}
}


