using Koishibot.Core.Features.ChannelPoints.Interfaces;
namespace Koishibot.Core.Features.ChannelPoints;

public record ChannelPointStatusService(
	IOptions<Settings> Settings,
	IDragonEggQuestService DragonEggQuestService
	) : IChannelPointStatusService
{
	public async Task Enable()
	{
		if (Settings.Value.DebugMode)
		{
			await DragonEggQuestService.Initialize();
			return;
		}

	}
}