using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Enums;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.HypeTrain;

namespace Koishibot.Core.Features.Supports.Events;

// == ⚫ HANDLER == //

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelhype_trainbegin">Twitch Documentation</see><br/>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#hype-train-begin-event"/>Eventsub Payload</see><br/>
/// <see href="https://dashboard.twitch.tv/monetization/community/hype-train">HypeTrain Settings</see><br/> 
/// When a hype train starts (100 bits, subs), posts message in chat and update overlay.
/// </summary>
public record HypeTrainStartedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	IChatReplyService ChatReplyService,
	KoishibotDbContext Database
	) : IRequestHandler<HypeTrainStartedCommand>
{
	public async Task Handle(HypeTrainStartedCommand command, CancellationToken cancel)
	{
		var data = command.CreateData();
		await ChatReplyService.App(Command.HypeTrainStarted, data);

		var hypeTrainVm = command.CreateVm();
		await Signalr.SendStreamEvent(hypeTrainVm);

		// Display list of available emotes on overlay
		// Show train on overlay with timer using the ends at time and goal bar for next level
	}
}

// == ⚫ COMMAND == //

public record HypeTrainStartedCommand(
	HypeTrainEvent e
	) : IRequest
{
	public object CreateData()
	{
		return new { Points = (e.Goal - e.Progress) };
	}

	public StreamEventVm CreateVm()
	{
		return new StreamEventVm
		{
			EventType = StreamEventType.HypeTrain,
			Timestamp = (DateTimeOffset.UtcNow).ToString("yyyy-MM-dd HH:mm"),
			Message = "A hype train has started!"
		};
	}
};