using Koishibot.Core.Features.Polls.Extensions;
using Koishibot.Core.Features.Polls.Interfaces;
using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Services.TwitchEventSub.Extensions;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;

namespace Koishibot.Core.Features.Polls.Events;

// == ⚫ EVENT SUB == //

public class VoteRecieved(
	IOptions<Settings> Settings,
	EventSubWebsocketClient EventSubClient,
	ITwitchAPI TwitchApi,
	IServiceScopeFactory ScopeFactory
	) : IVoteRecieved
{
	public async Task SetupHandler()
	{
		EventSubClient.ChannelPollProgress += OnVoteReceived;
		await SubToEvent();
	}

	public async Task SubToEvent()
	{
		await TwitchApi.CreateEventSubBroadcaster
			("channel.poll.progress", "1", Settings);
	}

	private async Task OnVoteReceived(object sender, ChannelPollProgressArgs args)
	{
		using var scope = ScopeFactory.CreateScope();
		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

		await mediatr.Send(new VoteReceivedCommand(args));
	}
}

// == ⚫ COMMAND == //

public record VoteReceivedCommand
	(ChannelPollProgressArgs args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollprogress">Channel Poll Progress</see>
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public record VoteReceivedHandler
		(IAppCache Cache, IChatMessageService BotIrc,
		ISignalrService Signalr
		) : IRequestHandler<VoteReceivedCommand>
{
	public async Task Handle(VoteReceivedCommand command, CancellationToken cancel)
	{
		var e = command.args.ConvertToEvent();

		var poll = new CurrentPoll().ConvertFromEvent(e);
		Cache.AddPoll(poll);

		var pollVm = poll.ConvertToVm();
		await Signalr.SendPoll(pollVm);

		// Update Overlay that vote was received
		if (e.IsRaidPoll())
		{
			var raidPollVm = new RaidPollVm().Set(e.Choices);
			await Signalr.SendRaidPollVote(raidPollVm);
		}
	}
}

// == ⚫  == //

public record VoteReceivedEvent(
	string Id,
	string Title,
	DateTimeOffset StartedAt,
	DateTimeOffset EndingAt,
	Dictionary<string, int> Choices) : INotification
{
	public TimeSpan Duration => StartedAt - EndingAt;

	public bool IsRaidPoll()
	{
		return Title == "Who should we raid?";
	}
}