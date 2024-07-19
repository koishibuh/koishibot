using Koishibot.Core.Features.Dandle.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Persistence.Configurations;

public class DandleWordConfig
	 : IEntityTypeConfiguration<DandleWord>
{
	public void Configure(EntityTypeBuilder<DandleWord> builder)
	{
		builder.ToTable("DandleWords");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id);

		builder.HasIndex(p => p.Word)
					 .IsUnique();

		builder.Property(p => p.Word)
					 .HasMaxLength(5);

		builder.HasMany(p => p.DandleResults)
			.WithOne(p => p.DandleWord)
			.HasForeignKey(p => p.DandleWordId);
	}
}