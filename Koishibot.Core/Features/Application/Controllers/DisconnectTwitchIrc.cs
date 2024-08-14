using Koishibot.Core.Services.Twitch.Irc;
namespace Koishibot.Core.Features.Application.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
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

/*═══════════════════【 HANDLER 】═══════════════════*/
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

/*═══════════════════【 COMMAND 】═══════════════════*/
public record DisconnectTwitchIrcCommand : IRequest;