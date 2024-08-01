using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.ChannelPoints.Interfaces;
using Koishibot.Core.Features.Obs.Interfaces;
using Koishibot.Core.Features.StreamInformation.Extensions;
namespace Koishibot.Core.Features.StreamInformation;

// == ⚫ COMMAND == //

public record StreamReconnectCommand() : INotification;

// == ⚫ HANDLER == //

public record StreamReconnectHandler(
	IAppCache Cache,
	//IStreamSessionService StreamSessionService,
	IObsService ObsService, 
	IChannelPointStatusService ChannelPointStatusService
	) : INotificationHandler<StreamReconnectCommand>
{
	public async Task Handle
		(StreamReconnectCommand command, CancellationToken cancel)
	{
		await Cache.ClearAttendanceCache()
							 .UpdateStreamStatusOnline();

		await ObsService.StartWebsocket();

		await ChannelPointStatusService.Enable();

		//await StreamSessionService.CreateOrReloadStreamSession();

		//await AdBreakService.GetAdSchedule();
	}
}