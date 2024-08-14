using Koishibot.Core.Features.ChannelPoints.Interfaces;
using Koishibot.Core.Features.StreamInformation.Extensions;
using Koishibot.Core.Features.StreamInformation.Interfaces;
namespace Koishibot.Core.Features.StreamInformation;

/*═══════════════════【 HANDLER 】═══════════════════*/
public record StreamReconnectHandler(
	IAppCache Cache,
	IStreamSessionService StreamSessionService,
	IChannelPointStatusService ChannelPointStatusService
	) : INotificationHandler<StreamReconnectCommand>
{
	public async Task Handle
		(StreamReconnectCommand command, CancellationToken cancel)
	{
		await Cache.UpdateStreamStatusOnline();

		await ChannelPointStatusService.Enable();

		await StreamSessionService.CreateOrReloadStreamSession();

		// await AdBreakService.GetAdSchedule();
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record StreamReconnectCommand : INotification;