using Koishibot.Core.Services.Twitch.Irc.Interfaces;

namespace Koishibot.Core.Features.Application.Controllers;


// == ⚫ POST == //

public class ConnectTwitchIrcController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Services"])]
	[HttpPost("/api/twitch-irc")]
	public async Task<ActionResult> ConnectTwitchIrc()
	{
		await Mediator.Send(new ConnectTwitchIrcCommand());
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record ConnectTwitchIrcCommand(
	) : IRequest;

// == ⚫ HANDLER == //

public record ConnectTwitchIrcHandler(
	ITwitchIrcService TwitchIrcService
	) : IRequestHandler<ConnectTwitchIrcCommand>
{
	public async Task Handle
		(ConnectTwitchIrcCommand dto, CancellationToken cancel)
	{
		await TwitchIrcService.CreateWebSocket();
	}
}