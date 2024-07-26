//using Koishibot.Core.Features.AdBreak.Extensions;
//using Koishibot.Core.Features.AdBreak.Interfaces;
//using Koishibot.Core.Features.AttendanceLog.Extensions;
//using Koishibot.Core.Features.ChannelPoints.Interfaces;
//using Koishibot.Core.Features.Common.Models;
//using Koishibot.Core.Features.Obs.Interfaces;
//using Koishibot.Core.Features.StreamInformation.Extensions;
//using Koishibot.Core.Features.StreamInformation.Interfaces;
//using TwitchLib.EventSub.Websockets.Core.EventArgs.Stream;
//namespace Koishibot.Core.Features.StreamInformation.Events;

//// == ⚫ EVENT SUB == //

//public class StreamOnline(
//	IOptions<Settings> Settings,
//	EventSubWebsocketClient EventSubClient,
//	ITwitchAPI TwitchApi,
//	IServiceScopeFactory ScopeFactory
//	) : IStreamOnline
//{
//	public async Task SetupMethod()
//	{
//		EventSubClient.StreamOnline += OnStreamOnline;
//		await SubToEvent();
//	}

//	public async Task SubToEvent()
//	{
//		await TwitchApi.CreateEventSubBroadcaster
//			("stream.online", "1", Settings);
//	}

//	private async Task OnStreamOnline(object sender, StreamOnlineArgs args)
//	{
//		using var scope = ScopeFactory.CreateScope();
//		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

//		await mediatr.Send(new StreamOnlineCommand());
//	}
//}

//// == ⚫ COMMAND == //

//public record StreamOnlineCommand() : IRequest;

//// == ⚫ HANDLER == //

///// <summary>
///// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#streamonline">Stream Online EventSub Documentation</see></para>
///// </summary>
///// <param name="sender"></param>
///// <param name="e"></param>
//public record StreamOnlineHandler
//	(IAppCache Cache, ISignalrService Signalr,
//	IStreamSessionService StreamSessionService,
//	IObsService ObsService, IPomodoroTimer AdBreakService,
//	IChannelPointStatusService ChannelPointStatusService
//	) : IRequestHandler<StreamOnlineCommand>
//{
//	public async Task Handle(StreamOnlineCommand command, CancellationToken cancel)
//	{
//		await Cache.ClearAttendanceCache()
//			.UpdateStreamStatusOnline();

//		await ObsService.StartWebsocket();

//		var timer = new CurrentTimer().SetStartingSoon();
//		Cache.AddCurrentTimer(timer);

//		var vm = timer.ConvertToVm();
//		await Signalr.UpdateTimerOverlay(vm);

//		await ChannelPointStatusService.Enable();

//		await StreamSessionService.CreateOrReloadStreamSession();

//		// Todo: Enable stats
//	}
//}
