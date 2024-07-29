namespace Koishibot.Core.Features.AdBreak.Models;

public record AdScheduleDto(
	int AvailableSnoozeCount,
	DateTimeOffset GainNextSnoozeAt,
	DateTimeOffset NextAdScheduledAt,
	TimeSpan AdDurationInSeconds,
	DateTimeOffset LastAdPlayedAt,
	int RemainingPrerollFreeTimeInSeconds
	)
{
	/// <summary>
	/// Offset by 1 minute 
	/// </summary>
	public TimeSpan CalculateAdjustedTimeUntilNextAd()
	{
		return NextAdScheduledAt - (DateTimeOffset.UtcNow - TimeSpan.FromMinutes(1));
	}
}