using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;

namespace Koishibot.Core.Features.Polls.Interfaces;

public interface IPollEnded
{
	Task SetupHandler();
	Task SubToEvent();
	Task OnPollEnded(object sender, ChannelPollEndArgs args);
}

//public record PollEndedHandler
//	(IAppCache Cache, IChatMessageService BotIrc,
//	ISignalrService Signalr, IRaidPollProcessor RaidPollProcessor,
//	IPollRepo PollRepo
//	) : INotificationHandler<PollEndedEvent>
//{
//	public async Task Handle
//		(PollEndedEvent e, CancellationToken cancel)
//	{

//		if (e.IsRaidPoll())
//		{
//			await RaidPollProcessor.Start(e);
//			return;
//		}

//		var winner = e.DeterminePollWinner();

//		await BotIrc.PollResult(e.Title, winner);

//		await new CurrentPoll()
//								.ConvertFromEvent(e)
//								.SetInCache(Cache)
//								.ConvertToVm()
//								.SendToVue(Signalr);

//		var streamSessionId = Cache.GetCurrentStreamSessionId();

//		await e.ConvertToEntity(winner, streamSessionId)
//					 .AddToRepo(PollRepo);

//		// Todo: Update Overlay that poll ended 
//	}
//}