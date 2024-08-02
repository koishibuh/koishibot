using Koishibot.Core.Features.Supports.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Persistence.Configurations;


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