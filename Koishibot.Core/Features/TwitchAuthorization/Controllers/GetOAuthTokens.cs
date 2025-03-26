using Koishibot.Core.Features.ApplicationAuthentication.Models;
using Koishibot.Core.Features.ChatCommands.Extensions;
using System.Text.Json;
using Koishibot.Core.Features.TwitchAuthorization.Enums;
using Koishibot.Core.Features.TwitchAuthorization.Models;
using Koishibot.Core.Persistence;

namespace Koishibot.Core.Features.TwitchAuthorization.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/twitch-auth")]
public class GetOAuthTokensController : ApiControllerBase
{
	[HttpPost("token")]
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
IStartupTwitchServices StartupTwitchServices,
KoishibotDbContext database
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

	private async Task SaveTokens(ClientCredentialsTokenResponse response)
	{
		Settings.Value.StreamerTokens.SetExpirationTime(response.ExpiresIn);
		Settings.Value.StreamerTokens.AccessToken = response.AccessToken;
		Settings.Value.StreamerTokens.RefreshToken = response.RefreshToken;

		var result = await database.AppKeys
			.Where(x => x.Name == "StreamerToken" && x.Key == "RefreshToken").FirstOrDefaultAsync();

		if (result is not null)
		{
			result.Value = response.RefreshToken;
			await database.UpdateEntry(result);
		}
		else
		{
			var refreshToken = new AppKey{ Name = "StreamerToken", Key = "RefreshToken", Value = response.RefreshToken };
			await database.UpdateEntry(refreshToken);
		}
	}
}

/*═══════════════════【 QUERY 】═══════════════════*/
public record GetOAuthTokensQuery(string Code) : IRequest;