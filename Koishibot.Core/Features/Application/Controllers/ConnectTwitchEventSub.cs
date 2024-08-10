using Koishibot.Core.Services.Twitch.EventSubs;
namespace Koishibot.Core.Features.Application.Controllers;

// == ⚫ POST == //

public class ConnectTwitchEventSubController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Services"])]
	[HttpPost("/api/twitch-eventsub")]
	public async Task<ActionResult> ConnectTwitchEventSub()
	{
		await Mediator.Send(new ConnectTwitchEventSub());
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record ConnectTwitchEventSub(
	) : IRequest;

// == ⚫ HANDLER == //

public record ConnectTwitchEventSubHandler(
	ITwitchEventSubService TwitchEventSubService
	) : IRequestHandler<ConnectTwitchEventSub>
{
	public async Task Handle
		(ConnectTwitchEventSub dto, CancellationToken cancel)
	{
		await TwitchEventSubService.CreateWebSocket();
	}
}