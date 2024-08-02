using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Supports.Enums;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.HypeTrain;
namespace Koishibot.Core.Features.Supports.Events;

// == ⚫ HANDLER == //

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelhype_trainend">Twitch Documentation</see><br/>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#hype-train-end-event"/>Eventsub Payload</see><br/>
/// <see href="https://dashboard.twitch.tv/monetization/community/hype-train">HypeTrain Settings</see><br/> 
/// When the hype train ends.
/// </summary>
public record HypeTrainEndedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	IChatReplyService ChatReplyService,
	KoishibotDbContext Database
	) : IRequestHandler<HypeTrainEndedCommand>
{
	public async Task Handle(HypeTrainEndedCommand command, CancellationToken cancel)
	{
		var data = command.CreateData();
		await ChatReplyService.App(Command.HypeTrainEnded, data);

		var hypeTrain = command.CreateModel();
		await Database.UpdateEntry(hypeTrain);
		
		// Update train graphic
	}
}

// == ⚫ COMMAND == //

public record HypeTrainEndedCommand(
	HypeTrainEvent e
	) : IRequest
{
	public object CreateData()
	{
		return new { Level = e.Level };
	}

	public HypeTrain CreateModel()
	{
		return new HypeTrain
		{
			StartedAt = e.StartedAt,
			EndedAt = e.EndedAt ?? DateTimeOffset.UtcNow,
			LevelCompleted = e.Level
		};
	}
}