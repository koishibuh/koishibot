using Koishibot.Core.Services.Twitch.EventSubs;

namespace Koishibot.Core.Features.Application.Controllers;

// == ⚫ DELETE == //

public class DisconnectTwitchEventSubController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Services"])]
	[HttpDelete("/api/twitch-eventsub")]
	public async Task<ActionResult> DisconnectTwitchEventSub()
	{
		await Mediator.Send(new DisconnectTwitchEventSub());
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record DisconnectTwitchEventSub(
	) : IRequest;

// == ⚫ HANDLER == //

public record DisconnectTwitchEventSubHandler(
	ITwitchEventSubService TwitchEventSubService
	) : IRequestHandler<DisconnectTwitchEventSub>
{
	public async Task Handle
		(DisconnectTwitchEventSub dto, CancellationToken cancel)
	{
		await TwitchEventSubService.DisconnectWebSocket();
	}
}