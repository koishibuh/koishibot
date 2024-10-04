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
	public TimeSpan CalculateAdjustedTimeUntilNextAd()
	{
		var time = NextAdScheduledAt.Value.AddMinutes(-1);

		return NextAdScheduledAt.HasValue
			? time - DateTimeOffset.Now
			: throw new InvalidOperationException("NextAdScheduledAt is null");
	}
}