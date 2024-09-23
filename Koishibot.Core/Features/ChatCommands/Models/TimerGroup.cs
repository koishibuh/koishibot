using Koishibot.Core.Features.ChatCommands.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.ChatCommands.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class TimerGroup : IEntity
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
	public TimeSpan Interval { get; set; }

	public List<ChatCommand> Commands { get; } = [];
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public record TimerGroupConfig()
{
	public void Configure(EntityTypeBuilder<TimerGroup> builder)
	{
		builder.ToTable("TimerGroups");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder
		.HasIndex(p => p.Name)
		.IsUnique();

		builder.Property(p => p.Name);

		builder.Property(p => p.Description);
		builder.Property(p => p.Interval);
	}
}