using Koishibot.Core.Features.StreamInformation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Koishibot.Core.Persistence.Configurations;

public class YearlyQuarterConfig : IEntityTypeConfiguration<YearlyQuarter>
{
	public void Configure(EntityTypeBuilder<YearlyQuarter> builder)
	{
		builder.ToTable("YearlyQuarters");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.StartDate);

		builder.Property(p => p.EndDate);
	}
}