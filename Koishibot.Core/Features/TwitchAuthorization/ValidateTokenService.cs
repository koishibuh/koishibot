using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Converters;
using System.Net.Http.Headers;
using System.Text.Json;
namespace Koishibot.Core.Features.TwitchAuthorization;

/*═══════════════════【 SERVICE 】═══════════════════*/
/// <summary>
/// <see cref="http://dev.twitch.tv/docs/authentication/validate-tokens/"/>
/// </summary>
public record ValidateTokenService(
IOptions<Settings> Settings,
IRefreshAccessTokenService RefreshAccessTokenService,
IHttpClientFactory HttpClientFactory
) : IValidateTokenService
{
	public async Task Start()
	{
		var baseClient = HttpClientFactory.CreateClient("Default");
		var uri = new Uri("https://id.twitch.tv/oauth2/validate");
		var httpClient = SetupHeaders(baseClient);
		var request = new HttpRequestMessage(HttpMethod.Get, uri);

		using var response = await httpClient.SendAsync(request);
		var content = await response.Content.ReadAsStringAsync();
		if (response.IsSuccessStatusCode)
		{
			var token = JsonSerializer.Deserialize<ValidateTokenResponse>(content)
			            ?? throw new Exception("Unable to get user from Validation Token");

			Settings.Value.StreamerTokens.UserId = token.UserId ?? string.Empty;
			Settings.Value.StreamerTokens.UserLogin = token.UserLogin ?? string.Empty;
		}
		else
		{
			await RefreshAccessTokenService.Start();
		}
	}

	private HttpClient SetupHeaders(HttpClient httpClient)
	{
		var clientId = Settings.Value.TwitchAppSettings.ClientId;
		var accessToken = Settings.Value.StreamerTokens.AccessToken;
		var authenticationHeader = new AuthenticationHeaderValue("Bearer", accessToken);

		httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
		httpClient.DefaultRequestHeaders.Add("Client-Id", clientId);
		return httpClient;
	}
}

/*══════════════════【 RESPONSE 】══════════════════*/
public class ValidateTokenResponse
{
	[JsonPropertyName("client_id")]
	public string? ClientId { get; init; }

	[JsonPropertyName("login")]
	public string? UserLogin { get; init; }

	[JsonPropertyName("scopes")]
	List<string>? Scopes { get; init; }

	[JsonPropertyName("user_id")]
	public string? UserId { get; init; }

	[JsonPropertyName("expires_in")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset? ExpiresAt { get; init; }
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IValidateTokenService
{
	Task Start();
}