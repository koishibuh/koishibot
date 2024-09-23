using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.HypeTrain;

namespace Koishibot.Core.Features.Supports.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelhype_trainprogress">Twitch Documentation</see><br/>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#hype-train-progress-event"/>Eventsub Payload</see><br/>
/// <see href="https://dashboard.twitch.tv/monetization/community/hype-train">HypeTrain Settings</see><br/> 
/// When an event is received to progress the hype train.
/// </summary>
public record HypeTrainProgressedHandler(
IAppCache Cache,
ISignalrService Signalr,
ITwitchUserHub TwitchUserHub,
KoishibotDbContext Database
) : IRequestHandler<HypeTrainProgressedCommand>
{
	public async Task Handle(HypeTrainProgressedCommand command, CancellationToken cancel)
	{
		// Update overlay goalbar for next level
		// Check if ends at timer needs to be updated - should change when next level is reached
		// Update Train graphic when new user is added
		await Task.CompletedTask;
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record HypeTrainProgressedCommand(
HypeTrainProgressedEvent args
) : IRequest
{
};