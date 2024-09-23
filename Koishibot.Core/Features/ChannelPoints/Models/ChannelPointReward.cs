using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.ChannelPoints.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class ChannelPointReward
{
	public int Id { get; set; }
	public DateTimeOffset CreatedOn { get; set; }
	public string TwitchId { get; set; } = null!;
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public int Cost { get; set; }
	public string BackgroundColor { get; set; } = string.Empty;
	public bool IsEnabled { get; set; }
	public bool IsUserInputRequired { get; set; }
	public bool IsMaxPerStreamEnabled { get; set; }
	public int MaxPerStream { get; set; }
	public bool IsMaxPerUserPerStreamEnabled { get; set; }
	public int MaxPerUserPerStream { get; set; }
	public bool IsGlobalCooldownEnabled { get; set; }
	public int GlobalCooldownSeconds { get; set; }
	public bool IsPaused { get; set; }
	public bool ShouldRedemptionsSkipRequestQueue { get; set; }
	public string ImageUrl { get; set; } = string.Empty;

	public ICollection<ChannelPointRedemption> ChannelPointRedemptions { get; set; } = [];

	// == ⚫ == //
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class ChannelPointRewardConfig : IEntityTypeConfiguration<ChannelPointReward>
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