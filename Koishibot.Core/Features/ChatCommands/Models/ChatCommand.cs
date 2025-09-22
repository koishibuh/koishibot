using Koishibot.Core.Features.ChatCommands.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Koishibot.Core.Features.ChatCommands.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class ChatCommand : IEntity
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public string Category { get; set; } = CommandCategory.Misc;
	public string PermissionLevel { get; set; } = Models.PermissionLevel.Everyone;
	public bool Enabled { get; set; }
	public TimeSpan UserCooldown { get; set; }
	public TimeSpan GlobalCooldown { get; set; }

	public ChatResponse? Response { get; set; }
	
	public List<TimerGroup>? TimerGroups { get; set; } = [];
	public int? ResponseId { get; set; }
}

public record NewChatCommandDto(
int Id,
string Name,
string Category,
bool Enabled, 
string Permissions,
TimeSpan UserCooldown,
DateTimeOffset? UserCooldownUp,
TimeSpan GlobalCooldown,
DateTimeOffset? GlobalCooldownUp,
int? ResponseId)
{
	public DateTimeOffset? userCooldownUp { get; set; } = UserCooldownUp;
	public DateTimeOffset? globalCooldownUp { get; set; } = GlobalCooldownUp;
};


/*══════════════════【 CONFIGURATION 】═════════════════*/
public class NewChatCommandConfig : IEntityTypeConfiguration<ChatCommand>
{
	public void Configure(EntityTypeBuilder<ChatCommand> builder)
	{
		builder.ToTable("ChatCommands");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.Name)
			.IsRequired()
			.HasMaxLength(100);
		
		builder
			.HasIndex(p => p.Name)
			.IsUnique();

		builder.Property(p => p.Category)
			.HasMaxLength(25);
		
		builder
			.Property(p => p.PermissionLevel)
			.HasMaxLength(25);
		
		builder.Property(p => p.Enabled);
		builder.Property(p => p.UserCooldown);
		builder.Property(p => p.GlobalCooldown);
		
		builder
			.HasMany(p => p.TimerGroups)
			.WithMany(p => p.ChatCommands)
			.UsingEntity<CommandTimerGroup>();
	}
}