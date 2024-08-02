using Koishibot.Core.Features.Supports.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Persistence.Configurations;

public class SubscriptionConfig : IEntityTypeConfiguration<Subscription>
{
	public void Configure(EntityTypeBuilder<Subscription> builder)
	{
		builder.ToTable("Subscriptions");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.Timestamp);

		builder.Property(p => p.UserId);

		builder.Property(p => p.Tier);

		builder.Property(p => p.Message);

		builder.Property(p => p.Gifted);
	}
}