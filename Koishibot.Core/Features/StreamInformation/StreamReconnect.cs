using Koishibot.Core.Features.AdBreak;
using Koishibot.Core.Features.ChannelPoints.Interfaces;
using Koishibot.Core.Features.StreamInformation.Extensions;
namespace Koishibot.Core.Features.StreamInformation;

/*═══════════════════【 HANDLER 】═══════════════════*/
public record StreamReconnectHandler(
IAppCache Cache,
IStreamSessionService StreamSessionService,
IPomodoroTimer PomodoroTimer,
IChannelPointStatusService ChannelPointStatusService
) : INotificationHandler<StreamReconnectCommand>
{
	public async Task Handle
		(StreamReconnectCommand command, CancellationToken cancel)
	{
		await Cache.UpdateStreamStatusOnline();
		// await ChannelPointStatusService.Enable();
		await StreamSessionService.Reconnect();
		// await PomodoroTimer.GetAdSchedule();
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record StreamReconnectCommand : INotification;