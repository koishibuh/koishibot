using TwitchLib.Api.Helix.Models.Channels.GetAdSchedule;

namespace Koishibot.Core.Features.AdBreak.Models;

public class AdBreakInfo
{
	public TimeSpan AdDurationInSeconds;
	public DateTimeOffset LastAdAt;
	public DateTimeOffset NextAdAt;
	public TimeSpan TimeUntilNextAd;

	public AdBreakInfo Set(GetAdScheduleResponse response)
	{
		var info = response.Data[0];

		AdDurationInSeconds = TimeSpan.FromSeconds(info.Duration); // 3 minutes
		LastAdAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(info.LastAdAt));
		NextAdAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(info.NextAdAt)) - TimeSpan.FromMinutes(1);

		return this;
	}

	public AdBreakInfo Set()
	{
		AdDurationInSeconds = TimeSpan.FromMinutes(3);
		LastAdAt = DateTimeOffset.Now;
		NextAdAt = DateTimeOffset.Now + TimeSpan.FromMinutes(58);

		return this;
	}

	public TimeSpan CalculateTimeUntilNextAd()
	{
		return NextAdAt - DateTimeOffset.Now;
	}
}
