namespace Koishibot.Core.Features.ChannelPoints.Models;

public record ChannelPointRewardVm(
	string TwitchId,
	string Title,
	int Cost,
	bool IsEnabled,
	bool IsUserInputRequired,
	string Description,
	string BackgroundColor,
	bool IsMaxPerStreamEnabled,
	int MaxPerStream,
	bool IsMaxPerUserPerStreamEnabled,
	int MaxPerUserPerStream,
	bool IsGlobalCooldownEnabled,
	int GlobalCooldownSeconds,
	bool IsPaused,
	bool SkipRequestQueue,
	string ImageUrl
);