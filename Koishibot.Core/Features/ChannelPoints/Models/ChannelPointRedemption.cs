using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.Twitch.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.ChannelPoints.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class ChannelPointRedemption : IEntity
{
	public int Id { get; set; }
	public int ChannelPointRewardId { get; set; }
	public DateTimeOffset Timestamp { get; set; }
	public int UserId { get; set; }
	public bool WasSuccesful { get; set; }
	public ChannelPointReward ChannelPointReward { get; set; } = null!;
	public TwitchUser TwitchUser { get; set; } = null!;

/*══════════════════【】═════════════════*/
	public ChannelPointRedemption Set(ChannelPointReward reward,
			TwitchUser user, DateTimeOffset redeemedAt, bool wasSuccesful)
	{
		ChannelPointRewardId = reward.Id;
		Timestamp = redeemedAt;
		UserId = user.Id;
		WasSuccesful = wasSuccesful;
		return this;
	}
}


public record ChannelPointRedemptionVm();

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class ChannelPointRedemptionConfig
: IEntityTypeConfiguration<ChannelPointRedemption>
{
	public void Configure(EntityTypeBuilder<ChannelPointRedemption> builder)
	{
		builder.ToTable("ChannelPointRedemptions");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id);

		builder.Property(p => p.ChannelPointRewardId);

		builder.Property(p => p.Timestamp);

		builder.Property(p => p.UserId);

		builder.Property(p => p.WasSuccesful);

		builder.HasOne(p => p.ChannelPointReward)
		.WithMany(p => p.ChannelPointRedemptions)
		.HasForeignKey(p => p.ChannelPointRewardId);

		builder.HasOne(p => p.TwitchUser)
		.WithMany(p => p.RedeemedChannelPointRewards)
		.HasForeignKey(p => p.UserId)
		.IsRequired();
	}
}