using Koishibot.Core.Services.Twitch.Irc.Interfaces;

namespace Koishibot.Core.Features.Application.Controllers;

// == ⚫ DELETE == //

public class DisconnectTwitchIrcController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Services"])]
	[HttpDelete("/api/twitch-irc")]
	public async Task<ActionResult> DisconnectTwitchIrc()
	{
		await Mediator.Send(new DisconnectTwitchIrcCommand());
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record DisconnectTwitchIrcCommand(
	) : IRequest;

// == ⚫ HANDLER == //

public record DisconnectTwitchIrcHandler(
	ITwitchIrcService TwitchIrcService
	) : IRequestHandler<DisconnectTwitchIrcCommand>
{
	public async Task Handle
		(DisconnectTwitchIrcCommand dto, CancellationToken cancel)
	{
		await TwitchIrcService.DisconnectWebSocket();
	}
}