namespace Koishibot.Core.Features.TwitchAuthorization.Controllers;


// == ⚫ GET == //

public class GetAuthorizationUrlController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Twitch Oauth"])]
	[HttpGet("/api/twitch-auth/url")]
	public async Task<ActionResult> GetAuthorizationUrlCommand()
	{
		var result = await Mediator.Send(new GetAuthorizationUrlQuery());
		return Ok(result);
	}
}

// == ⚫ QUERY == //

public record GetAuthorizationUrlQuery() : IRequest<string>;

// == ⚫ HANDLER == //

public class GetAuthorizationUrlHandler(
	IOptions<Settings> Settings
	) : IRequestHandler<GetAuthorizationUrlQuery, string>
{
	public TwitchAppSettings settings => Settings.Value.TwitchAppSettings;

	public async Task<string> Handle(GetAuthorizationUrlQuery query, CancellationToken cancel)
	{
		return await Task.FromResult("https://id.twitch.tv/oauth2/authorize?" +
			$"client_id={settings.ClientId}&" +
			$"redirect_uri={settings.RedirectUri}&" +
			$"response_type=code&" +
			$"force_verify=true&" +
			$"state={settings.CreateStateVerify()}&" +
			$"scope={string.Join("+", ScopesStreamer.Scopes)}");
	}
}