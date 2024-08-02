using Koishibot.Core.Features.Supports.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Persistence.Configurations;


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