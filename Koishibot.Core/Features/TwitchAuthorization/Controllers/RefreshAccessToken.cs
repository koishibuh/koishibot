namespace Koishibot.Core.Features.TwitchAuthorization.Controllers;

// == ⚫ POST == //

public class RefreshAccessTokenController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Twitch Oauth"])]
	[HttpPut("/api/twitch-auth")]
	public async Task<ActionResult> RefreshAccessToken()
	{
		await Mediator.Send(new RefreshAccessTokenCommand());
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record RefreshAccessTokenCommand() : IRequest;

// == ⚫ HANDLER == //

public record RefreshAccessTokenHandler(
		IRefreshAccessTokenService RefreshAccessTokenService
		) : IRequestHandler<RefreshAccessTokenCommand>
{
	public async Task Handle
			(RefreshAccessTokenCommand command, CancellationToken cancel)
	{
		try
		{
			await RefreshAccessTokenService.Start();
		}
		catch (Exception ex)
		{
			throw new Exception("Unable to refresh tokens", ex);
		}
	}
}