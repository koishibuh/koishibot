using Koishibot.Core.Features.ChatCommands.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.Supports.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class HypeTrain : IEntity
{
	public int Id { get; set; }
	public DateTimeOffset StartedAt { get; set; }
	public DateTimeOffset EndedAt { get; set; }
	public int LevelCompleted { get; set; }
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class HypeTrainConfigConfig: IEntityTypeConfiguration<HypeTrain>
{
	public void Configure(EntityTypeBuilder<HypeTrain> builder)
	{
		builder.ToTable("HypeTrain");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.StartedAt);
		builder.Property(p => p.EndedAt);
		builder.Property(p => p.LevelCompleted);
	}
}