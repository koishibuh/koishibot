using Koishibot.Core.Features.ChannelPoints.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Persistence.Configurations;
public class ChannelPointRedemptionConfig
	: IEntityTypeConfiguration<ChannelPointRedemption>
{
	public void Configure(EntityTypeBuilder<ChannelPointRedemption> builder)
	{
		builder.ToTable("ChannelPointRedemptions");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id);

		builder.Property(p => p.ChannelPointRewardId);

		builder.Property(p => p.RedeemedAt);

		builder.Property(p => p.UserId);

		builder.Property(p => p.WasSuccesful);

		builder.HasOne(p => p.ChannelPointReward)
			.WithMany(p => p.ChannelPointRedemptions)
			.HasForeignKey(p => p.ChannelPointRewardId);

		builder.HasOne(p => p.User)
			.WithMany(p => p.RedeemedChannelPointRewards)
			.HasForeignKey(p => p.UserId)
			.IsRequired();
	}
}
