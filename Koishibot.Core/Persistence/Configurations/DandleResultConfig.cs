using Koishibot.Core.Features.Dandle.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Koishibot.Core.Persistence.Configurations;

public class DandleResultConfig
	 : IEntityTypeConfiguration<DandleResult>
{
	public void Configure(EntityTypeBuilder<DandleResult> builder)
	{
		builder.ToTable("DandleResults");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id);

		builder.Property(p => p.TwitchStreamId);

		builder.Property(p => p.Timestamp);

		builder.Property(p => p.DandleWordId);

		builder.Property(p => p.GuessCount);
	}
}