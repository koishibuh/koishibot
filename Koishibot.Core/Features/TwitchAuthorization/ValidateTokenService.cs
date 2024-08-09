using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Converters;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Koishibot.Core.Features.TwitchAuthorization;

/// <summary>
/// <see cref="https://dev.twitch.tv/docs/authentication/validate-tokens/"/>
/// </summary>
public record ValidateTokenService(
	IOptions<Settings> Settings,
	IHttpClientFactory HttpClientFactory) : IValidateTokenService
{
	// Validate token every hour
	public async Task Start()
	{
		var clientId = Settings.Value.TwitchAppSettings.ClientId;
		var accessToken = Settings.Value.StreamerTokens.AccessToken;
		var authenticationHeader = new AuthenticationHeaderValue("Bearer", accessToken);

		var httpClient = HttpClientFactory.CreateClient("Default");
		var uri = new Uri("https://id.twitch.tv/oauth2/validate");

		httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
		httpClient.DefaultRequestHeaders.Add("Client-Id", clientId);

		var request = new HttpRequestMessage(HttpMethod.Get, uri);	
		using var response = await httpClient.SendAsync(request);

		var content = await response.Content.ReadAsStringAsync();
		if (!response.IsSuccessStatusCode)
		{
			var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
			// custom exception here
			throw new Exception($"Api request failed, {errorResponse?.Message}");
		}

		var result = JsonSerializer.Deserialize<ValidateTokenResponse>(content) 
			?? throw new Exception("Unable to get user from Validation Token");

		Settings.Value.StreamerTokens.UserId = result.UserId ?? string.Empty;
		Settings.Value.StreamerTokens.UserLogin = result.Login ?? string.Empty;
	}
}

public class ValidateTokenResponse
{
	[JsonPropertyName("client_id")]
	public string? ClientId { get; set; }

	[JsonPropertyName("login")]
	public string? Login { get; set; }

	[JsonPropertyName("scopes")]
	public List<string>? Scopes { get; set; }

	[JsonPropertyName("user_id")]
	public string? UserId { get; set; }

	[JsonPropertyName("expires_in")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset ExpiresAt { get; set; }
}

public interface IValidateTokenService
{
	Task Start();
}