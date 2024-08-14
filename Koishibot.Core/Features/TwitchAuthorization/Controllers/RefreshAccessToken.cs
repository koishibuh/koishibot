namespace Koishibot.Core.Features.TwitchAuthorization.Controllers;

/*--------------------< CONTROLLER >-----------------------*/
public class RefreshAccessTokenController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Twitch Oauth"])]
	[HttpPost("/api/twitch-auth")]
	public async Task<ActionResult> RefreshAccessToken
		([FromBody] RefreshAccessTokenCommand command)
	{
		await Mediator.Send(command);
		return Ok();
	}
}

/*-----------------------< HANDLER >-----------------------*/
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

/*----------------------< QUERY >-------------------------*/
public record RefreshAccessTokenCommand(string Token) : IRequest;