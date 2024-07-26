using Koishibot.Core.Features.TwitchAuthorization.Models;
using Koishibot.Core.Services.Twitch.EventSubs;
using Newtonsoft.Json;
namespace Koishibot.Core.Features.TwitchAuthorization.Controllers;

// == ⚫ GET == //

public class GetOAuthTokensController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Twitch Oauth"])]
	[HttpPost("/api/twitch-auth/token")]
	public async Task<ActionResult> GetOAuthTokens([FromBody] GetOAuthTokensQuery query)
	{
		await Mediator.Send(query);
		return Ok();
	}
}

// == ⚫ QUERY == //

public record GetOAuthTokensQuery(
	string Code
	) : IRequest;

// == ⚫ HANDLER == //

public class GetOAuthTokensHandler(
	IOptions<Settings> Settings,
	IHttpClientFactory HttpClientFactory,
	ITwitchEventSubService TwitchEventSubService
	//ITwitchAPI TwitchApi, ITwitchEventSubHub TwitchEventSubHub,
	//IStreamerIrcHub StreamerIrc, IBotIrcHub BotIrc
	//IOAuthTokensProcessor OAuthTokensProcessor
	)
	: IRequestHandler<GetOAuthTokensQuery>
{
	public TwitchAppSettings settings => Settings.Value.TwitchAppSettings;

	public async Task Handle(GetOAuthTokensQuery query, CancellationToken cancel)
	{
		var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
		{
			new("client_id", Settings.Value.TwitchAppSettings.ClientId),
			new("client_secret", Settings.Value.TwitchAppSettings.ClientSecret),
			new("grant_type", "authorization_code"),
			new("code", query.Code),
			new("redirect_uri", Settings.Value.TwitchAppSettings.RedirectUri)
		});

		var httpClient = HttpClientFactory.CreateClient("Default");
		using var response = await httpClient.PostAsync(new Uri("https://id.twitch.tv/oauth2/token"), content);

		if (response.IsSuccessStatusCode)
		{
			var responseBody = await response.Content.ReadAsStringAsync();

			var result = JsonConvert.DeserializeObject<ClientCredentialsTokenResponse>(responseBody)
				?? throw new Exception("An error occurred while generating a twitch token.");

			//TwitchApi.Settings.AccessToken = result.AccessToken;
			Settings.Value.StreamerTokens.SetExpirationTime(result.ExpiresIn);
			Settings.Value.StreamerTokens.AccessToken = result.AccessToken;
			Settings.Value.StreamerTokens.RefreshToken = result.RefreshToken;

			await TwitchEventSubService.CreateWebSocket(cancel);

			//await Task.WhenAll(
			//		TwitchEventSubHub.Start(),
			//		StreamerIrc.Start(),
			//		BotIrc.Start()
			//		);
		}
	}
}