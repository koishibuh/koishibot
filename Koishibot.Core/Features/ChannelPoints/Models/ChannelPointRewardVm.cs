namespace Koishibot.Core.Features.ChannelPoints.Models;

public record ChannelPointRewardVm(
	int Id,
	string Title,
	int Cost,
	bool IsEnabled,
	bool IsUserInputRequired,
	string Prompt,
	string BackgroundColor,
	bool IsMaxPerStreamEnabled,
	int MaxPerStream,
	bool IsMaxPerUserPerStreamEnabled,
	int MaxPerUserPerStream,
	bool IsGlobalCooldownEnabled,
	int GlobalCooldownSeconds,
	bool SkipRequestQueue
);