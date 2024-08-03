using Koishibot.Core.Features.Supports.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Persistence.Configurations;


public class SETipConfigConfig : IEntityTypeConfiguration<SETip>
{
	public void Configure(EntityTypeBuilder<SETip> builder)
	{
		builder.ToTable("SETips");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.StreamElementsId);
		builder.Property(p => p.Timestamp);
		builder.Property(p => p.UserId);
		builder.Property(p => p.Username);
		builder.Property(p => p.Message);
		builder.Property(p => p.Amount);
	}
}