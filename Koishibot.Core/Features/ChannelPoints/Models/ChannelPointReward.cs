namespace Koishibot.Core.Features.ChannelPoints.Models;

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