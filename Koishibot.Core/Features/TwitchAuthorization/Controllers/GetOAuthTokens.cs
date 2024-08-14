using System.Text.Json;
using Koishibot.Core.Features.TwitchAuthorization.Enums;
using Koishibot.Core.Features.TwitchAuthorization.Models;
namespace Koishibot.Core.Features.TwitchAuthorization.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
public class GetOAuthTokensController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Twitch Oauth"])]
	[HttpPost("/api/twitch-auth/token")]
	public async Task<ActionResult> GetOAuthTokens
	([FromBody] GetOAuthTokensQuery query)
	{
		await Mediator.Send(query);
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetOAuthTokensHandler(
IOptions<Settings> Settings,
IHttpClientFactory HttpClientFactory,
IValidateTokenService ValidateTokenService,
IStartupTwitchServices StartupTwitchServices
) : IRequestHandler<GetOAuthTokensQuery>
{
	public async Task Handle(GetOAuthTokensQuery query, CancellationToken cancel)
	{
		var requestContent = CreateRequestContent(query.Code);

		var httpClient = HttpClientFactory.CreateClient("Default");
		var uri = new Uri("https://id.twitch.tv/oauth2/token");

		using var response = await httpClient.PostAsync(uri, requestContent, cancel);
		if (response.IsSuccessStatusCode is false)
		{
			throw new Exception("Unable to get oauth tokens");
		}

		var content = await response.Content.ReadAsStringAsync(cancel);
		var result = JsonSerializer.Deserialize<ClientCredentialsTokenResponse>(content)
		             ?? throw new Exception("An error occurred while generating a twitch token.");

		SaveTokens(result);

		await ValidateTokenService.Start();
		await StartupTwitchServices.Start();
	}

	private FormUrlEncodedContent CreateRequestContent(string code)
	{
		return new FormUrlEncodedContent(
		[
		new("client_id", Settings.Value.TwitchAppSettings.ClientId),
		new("client_secret", Settings.Value.TwitchAppSettings.ClientSecret),
		new("grant_type", GrantType.AuthorizationCode.ConvertToString()),
		new("code", code),
		new("redirect_uri", Settings.Value.TwitchAppSettings.RedirectUri)
		]);
	}

	private void SaveTokens(ClientCredentialsTokenResponse response)
	{
		Settings.Value.StreamerTokens.SetExpirationTime(response.ExpiresIn);
		Settings.Value.StreamerTokens.AccessToken = response.AccessToken;
		Settings.Value.StreamerTokens.RefreshToken = response.RefreshToken;
	}
}

/*═══════════════════【 QUERY 】═══════════════════*/
public record GetOAuthTokensQuery(string Code) : IRequest;