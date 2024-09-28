using Koishibot.Core.Features.ChannelPoints.Interfaces;
namespace Koishibot.Core.Features.ChannelPoints;

public record ChannelPointStatusService(
IOptions<Settings> Settings,
IDragonQuestService DragonQuestService
) : IChannelPointStatusService
{
	public async Task Enable()
	{
		if (TodayIsMondayOrThursday())
		{
			await DragonQuestService.Initialize();
		}
	}

	private static bool TodayIsMondayOrThursday() =>
		DateTime.UtcNow.DayOfWeek is DayOfWeek.Monday or DayOfWeek.Thursday;
}