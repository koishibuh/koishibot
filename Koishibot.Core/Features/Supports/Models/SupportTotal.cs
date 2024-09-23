using Koishibot.Core.Features.TwitchUsers.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.Supports.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class SupportTotal
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public int MonthsSubscribed { get; set; }
	public int SubsGifted { get; set; }
	public int BitsCheered { get; set; }
	public int Tipped { get; set; }

	// == ⚫ NAVIGATION == //

	public TwitchUser TwitchUser { get; set; } = null!;
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class SupportTotalConfig : IEntityTypeConfiguration<SupportTotal>
{
	public void Configure(EntityTypeBuilder<SupportTotal> builder)
	{
		builder.ToTable("SupportTotals");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.UserId);

		builder.Property(p => p.MonthsSubscribed);

		builder.Property(p => p.SubsGifted);

		builder.Property(p => p.BitsCheered);

		builder.Property(p => p.Tipped);
	}
}