using Koishibot.Core.Features.ChannelPoints.Interfaces;
namespace Koishibot.Core.Features.ChannelPoints;

public record ChannelPointStatusService(
	IOptions<Settings> Settings,
	IDragonEggQuestService DragonEggQuestService
	) : IChannelPointStatusService
{
	public async Task Enable()
	{
		if (TodayIsMondayOrThursday())
		{
			await DragonEggQuestService.Initialize();
		}
	}

	private bool TodayIsMondayOrThursday()
	{
		var today = DateTime.UtcNow;

		return today.DayOfWeek is
		DayOfWeek.Monday or
		DayOfWeek.Thursday;
	}
}