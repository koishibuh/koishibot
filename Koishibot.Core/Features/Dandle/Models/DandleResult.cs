using Koishibot.Core.Features.StreamInformation.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.Dandle.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class DandleResult
{
	public int Id { get; set; }
	public DateTimeOffset Timestamp { get; set; }
	public int DandleWordId { get; set; }
	public int GuessCount { get; set; }

	public DandleWord DandleWord { get; set; } = null!;
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class DandleResultConfig : IEntityTypeConfiguration<DandleResult>
{
	public void Configure(EntityTypeBuilder<DandleResult> builder)
	{
		builder.ToTable("DandleResults");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id);

		builder.Property(p => p.Timestamp);

		builder.Property(p => p.DandleWordId);

		builder.Property(p => p.GuessCount);
	}
}