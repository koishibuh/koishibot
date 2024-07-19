using Koishibot.Core.Features.RaidSuggestions.Interfaces;

namespace Koishibot.Core.Features.StreamInformation.Models;

public record LiveStreamInfo(
string TwitchUserId,
string StreamId,
string GameId,
string GameName,
string Title,
int ViewerCount,
DateTime StartedAt,
string ThumbnailUrl
)
{
	public TwitchStream CreateTwitchStream(bool attendanceStatus, YearlyQuarter yearlyQuarter)
	{
		return new TwitchStream
		{
			StreamId = StreamId,
			StartedAt = StartedAt,
			AttendanceMandatory = attendanceStatus,
			YearlyQuarter = yearlyQuarter
		};
	}

	public bool StreamerOverMaxViewerCount(int maxViewerCount)
	{
		return ViewerCount >= maxViewerCount;
	}

};
