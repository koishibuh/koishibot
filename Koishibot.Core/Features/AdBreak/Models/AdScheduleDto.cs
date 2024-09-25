namespace Koishibot.Core.Features.AdBreak.Models;

public record AdScheduleDto(
int AvailableSnoozeCount,
DateTimeOffset? GainNextSnoozeAt,
DateTimeOffset? NextAdScheduledAt,
TimeSpan AdDurationInSeconds,
DateTimeOffset? LastAdPlayedAt,
int RemainingPrerollFreeTimeInSeconds
)
{
	/// <summary>
	/// Offset by 1 minute 
	/// </summary>
	public TimeSpan CalculateAdjustedTimeUntilNextAd() =>
		NextAdScheduledAt.HasValue
			? NextAdScheduledAt.Value - DateTimeOffset.UtcNow.AddMinutes(-1)
			: throw new InvalidOperationException("NextAdScheduledAt is null");
			// : TimeSpan.Zero;
}