namespace Koishibot.Core.Features.TwitchAuthorization.Controllers;

// == ⚫ GET == //

public class GetAuthorizationUrlController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Twitch Oauth"])]
	[HttpGet("/api/twitch-auth")]
	public async Task<ActionResult> GetAuthorizationUrlCommand()
	{
		var result = await Mediator.Send(new GetAuthorizationUrlQuery());
		return Ok(result);
	}
}

// == ⚫ QUERY == //

public record GetAuthorizationUrlQuery() : IRequest<string>;

// == ⚫ HANDLER == //

public class GetOAuthTokensHandler
	(IOptions<Settings> Settings, IOAuthTokensProcessor OAuthTokensProcessor)
	: IRequestHandler<GetAuthorizationUrlQuery, string>
{
	public TwitchAppSettings settings => Settings.Value.TwitchAppSettings;

	public async Task<string> Handle(GetAuthorizationUrlQuery query, CancellationToken cancel)
	{
		OAuthTokensProcessor.StartListener();

		return await Task.FromResult("https://id.twitch.tv/oauth2/authorize?" +
										$"client_id={settings.ClientId}&" +
										$"redirect_uri={settings.RedirectUri}&" +
										$"response_type=code&" +
										$"force_verify=true&" +
										$"state={settings.CreateStateVerify()}&" +
										$"scope={string.Join("+", ScopesStreamer.Scopes)}");
	}
}