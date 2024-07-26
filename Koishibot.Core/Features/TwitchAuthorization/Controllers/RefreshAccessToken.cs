namespace Koishibot.Core.Features.TwitchAuthorization.Controllers;

// == ⚫ POST == //

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

// == ⚫ COMMAND == //

public record RefreshAccessTokenCommand(string Token) : IRequest;

// == ⚫ HANDLER == //

public record RefreshAccessTokenHandler(
		//IRefreshAccessTokenService RefreshAccessTokenService,
		//ITwitchEventSubHub TwitchEventSubHub,
		//IStreamerIrcHub StreamerIrc, IBotIrcHub BotIrc
		) : IRequestHandler<RefreshAccessTokenCommand>
{
	public async Task Handle
			(RefreshAccessTokenCommand command, CancellationToken cancel)
	{
	//	try
	//	{
	//		var result = await RefreshAccessTokenService.StartWithToken(command.Token);
	//		if (result)
	//		{
	//			await Task.WhenAll(
	//				TwitchEventSubHub.Start(),
	//				StreamerIrc.Start(),
	//				BotIrc.Start()
	//				);
	//		}
	//	}
	//	catch (Exception ex)
	//	{
	//		throw new Exception("Unable to refresh tokens", ex);
	//	}
	}
}