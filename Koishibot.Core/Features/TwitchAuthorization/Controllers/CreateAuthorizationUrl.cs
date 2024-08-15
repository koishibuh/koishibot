namespace Koishibot.Core.Features.TwitchAuthorization.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/twitch-auth")]
public class GetAuthorizationUrlController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Twitch Oauth"])]
	[HttpGet("url")]
	public async Task<ActionResult> GetAuthorizationUrlCommand()
	{
		var result = await Mediator.Send(new GetAuthorizationUrlQuery());
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetAuthorizationUrlHandler(
IOptions<Settings> Settings
) : IRequestHandler<GetAuthorizationUrlQuery, string>
{
	public async Task<string> Handle(GetAuthorizationUrlQuery query, CancellationToken cancel)
	{
		var settings = Settings.Value.TwitchAppSettings;

		return await Task.FromResult("https://id.twitch.tv/oauth2/authorize?" +
			$"client_id={settings.ClientId}&" +
			$"redirect_uri={settings.RedirectUri}&" +
			"response_type=code&" +
			"force_verify=true&" +
			$"state={settings.CreateStateVerify()}&" +
			$"scope={string.Join("+", ScopesStreamer.Scopes)}");
	}
}

/*════════════════════【 QUERY 】════════════════════*/
public record GetAuthorizationUrlQuery : IRequest<string>;