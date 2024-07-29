using System.Net.Http.Headers;

namespace Koishibot.Core.Features.TwitchAuthorization;
public record ValidateTokenService(
	IOptions<Settings> Settings,
	IHttpClientFactory HttpClientFactory)
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
			//var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
			//// custom exception here
			//throw new Exception($"Api request failed, {errorResponse?.Message}");

			// Refresh tokens
		}
	}
}
