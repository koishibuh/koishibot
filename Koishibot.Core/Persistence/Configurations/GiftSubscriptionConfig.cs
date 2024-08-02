using Koishibot.Core.Features.Supports.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Persistence.Configurations;


public class GiftSubscriptionConfig : IEntityTypeConfiguration<GiftSubscription>
{
	public void Configure(EntityTypeBuilder<GiftSubscription> builder)
	{
		builder.ToTable("GiftSubscriptions");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.Timestamp);

		builder.Property(p => p.UserId);

		builder.Property(p => p.Tier);

		builder.Property(p => p.Total);
	}
}