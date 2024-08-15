namespace Koishibot.Core.Features.TwitchAuthorization.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/twitch-auth")]
public class RefreshAccessTokenController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Twitch Oauth"])]
	[HttpPost]
	public async Task<ActionResult> RefreshAccessToken
		([FromBody] RefreshAccessTokenCommand command)
	{
		await Mediator.Send(command);
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record RefreshAccessTokenHandler(
IRefreshAccessTokenService RefreshAccessTokenService
) : IRequestHandler<RefreshAccessTokenCommand>
{
	public async Task Handle
		(RefreshAccessTokenCommand command, CancellationToken cancel)
	{
		await RefreshAccessTokenService.Start();
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record RefreshAccessTokenCommand(string Token) : IRequest;