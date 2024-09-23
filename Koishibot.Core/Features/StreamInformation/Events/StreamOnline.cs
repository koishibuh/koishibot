using Koishibot.Core.Features.AdBreak.Extensions;
using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.ChannelPoints.Interfaces;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Dandle;
using Koishibot.Core.Features.StreamInformation.Extensions;
using Koishibot.Core.Features.StreamInformation.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.OBS;

namespace Koishibot.Core.Features.StreamInformation.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#streamonline">Stream Online EventSub Documentation</see></para>
/// </summary>
public record StreamOnlineHandler(
IAppCache Cache,
ISignalrService Signalr,
IStreamSessionService StreamSessionService,
IObsService ObsService,
IChannelPointStatusService ChannelPointStatusService,
IDandleService DandleService,
KoishibotDbContext Database
) : IRequestHandler<StreamOnlineCommand>
{
	public async Task Handle(StreamOnlineCommand command, CancellationToken cancel)
	{
		await StreamSessionService.CreateOrReloadStreamSession();

		await Cache
		.ClearAttendanceCache()
		.UpdateStreamStatusOnline();

		await ObsService.CreateWebSocket();

		var timer = new CurrentTimer().SetStartingSoon();
		Cache.AddCurrentTimer(timer);

		var vm = timer.ConvertToVm();
		await Signalr.UpdateTimerOverlay(vm);

		await ChannelPointStatusService.Enable();

		await DandleService.StartGame();

		// Todo: Enable stats
	}
}

/*════════════════════【 COMMAND 】════════════════════*/
public record StreamOnlineCommand: IRequest;