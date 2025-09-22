using Koishibot.Core.Features.AdBreak.Extensions;
using Koishibot.Core.Features.AttendanceLog.Models;
using Koishibot.Core.Features.ChannelPoints.Interfaces;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatCommands.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Dandle;
using Koishibot.Core.Features.Dandle.Extensions;
using Koishibot.Core.Features.StreamInformation.Extensions;
using Koishibot.Core.Services.OBS;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.StreamStatus;

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
IChatReplyService ChatReplyService
) : IRequestHandler<StreamOnlineCommand>
{
	public async Task Handle(StreamOnlineCommand command, CancellationToken cancel)
	{
		await StreamSessionService.CreateOrReloadStreamSession(command.e.StreamId, command.e.StartedAt);

		await Cache
			.ClearAttendanceCache()
			.UpdateStreamStatusOnline();

		await ObsService.CreateWebSocket();
		await InitializeTimer();

		await ChatReplyService.CreateResponse(Command.StreamOnline);

		await ChannelPointStatusService.Enable();
		if (Cache.DandleIsClosed())
		{
			await DandleService.StartGame();
		}

	}

	private async Task InitializeTimer()
	{
		var timer = new CurrentTimer().SetStartingSoon();
		Cache.AddCurrentTimer(timer);
		var vm = timer.ConvertToVm();
		await Signalr.UpdateTimerOverlay(vm);
	}
}

/*════════════════════【 COMMAND 】════════════════════*/
public record StreamOnlineCommand(StreamOnlineEvent e) : IRequest;