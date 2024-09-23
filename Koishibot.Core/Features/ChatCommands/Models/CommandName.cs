using Koishibot.Core.Features.ChatCommands.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.ChatCommands.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class CommandName : IEntity
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public int? ChatCommandId { get; set; }
	public ChatCommand? ChatCommand { get; set; }
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class AttendanceConfig : IEntityTypeConfiguration<CommandName>
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