using System.Text.Json;
using Koishibot.Core.Features.TwitchAuthorization.Enums;
using Koishibot.Core.Features.TwitchAuthorization.Models;
namespace Koishibot.Core.Features.TwitchAuthorization;

/*═══════════════════【 SERVICE 】═══════════════════*/
public record RefreshAccessTokenService(
IOptions<Settings> Settings,
IHttpClientFactory HttpClientFactory
) : IRefreshAccessTokenService
{
	public async Task Start()
	{
		var httpClient = HttpClientFactory.CreateClient("Default");
		var uri = new Uri("https://id.twitch.tv/oauth2/token");
		var requestContent = CreateRequestContent();

		using var response = await httpClient.PostAsync(uri, requestContent);
		if (response.IsSuccessStatusCode is false)
		{
			throw new Exception("Unable to get oauth tokens");
		}

		var content = await response.Content.ReadAsStringAsync();
		var result = JsonSerializer.Deserialize<ClientCredentialsTokenResponse>(content)
		             ?? throw new Exception("An error occurred while generating a twitch token.");

		SaveTokens(result);

#if DEBUG
		SaveSettingsToJson();
#endif
	}

	private FormUrlEncodedContent CreateRequestContent()
	{
		return new FormUrlEncodedContent(
		[
		new("client_id", Settings.Value.TwitchAppSettings.ClientId),
		new("client_secret", Settings.Value.TwitchAppSettings.ClientSecret),
		new("grant_type", GrantType.RefreshToken.ConvertToString()),
		new("refresh_token", Settings.Value.StreamerTokens.RefreshToken)
		]);
	}

	private void SaveTokens(ClientCredentialsTokenResponse response)
	{
		Settings.Value.StreamerTokens.SetExpirationTime(response.ExpiresIn);
		Settings.Value.StreamerTokens.AccessToken = response.AccessToken;
		Settings.Value.StreamerTokens.RefreshToken = response.RefreshToken;
	}

	private void SaveSettingsToJson()
	{
		var wrapper = new { AppSettings = Settings.Value };
		wrapper.AppSettings.TwitchEventSubSessionId = "";
		var options = new JsonSerializerOptions { WriteIndented = true };
		var updatedSettings = JsonSerializer.Serialize(wrapper, options);
		File.WriteAllText("ASettings/settings.json", updatedSettings);
	}
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IRefreshAccessTokenService
{
	Task Start();
}