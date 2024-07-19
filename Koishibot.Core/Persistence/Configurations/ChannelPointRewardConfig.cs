using Koishibot.Core.Features.ChannelPoints.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Persistence.Configurations;

public class ChannelPointRewardConfig
	: IEntityTypeConfiguration<ChannelPointReward>
{
	public void Configure(EntityTypeBuilder<ChannelPointReward> builder)
	{
		builder.ToTable("ChannelPointRewards");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id);

		builder.HasIndex(p => p.TwitchId);

		builder.Property(p => p.Title);
		builder.Property(p => p.Description);
		builder.Property(p => p.Cost);
		builder.Property(p => p.BackgroundColor);
		builder.Property(p => p.IsEnabled);
		builder.Property(p => p.IsUserInputRequired);
		builder.Property(p => p.IsMaxPerStreamEnabled);
		builder.Property(p => p.MaxPerStream);
		builder.Property(p => p.IsMaxPerUserPerStreamEnabled);
		builder.Property(p => p.MaxPerUserPerStream);
		builder.Property(p => p.IsGlobalCooldownEnabled);
		builder.Property(p => p.GlobalCooldownSeconds);
		builder.Property(p => p.IsPaused);
		builder.Property(p => p.ShouldRedemptionsSkipRequestQueue);
		builder.Property(p => p.ImageUrl);
	}
}