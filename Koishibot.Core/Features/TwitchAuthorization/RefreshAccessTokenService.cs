using Koishibot.Core.Features.TwitchAuthorization.Enums;
using Koishibot.Core.Features.TwitchAuthorization.Models;
using Newtonsoft.Json;

namespace Koishibot.Core.Features.TwitchAuthorization;
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
		var content = await response.Content.ReadAsStringAsync();

		var result = JsonConvert.DeserializeObject<ClientCredentialsTokenResponse>(content)
			?? throw new Exception("An error occurred while generating a twitch token.");

		SaveTokens(result);

#if DEBUG
		SaveSettingsToJson();
#endif
	}

	public FormUrlEncodedContent CreateRequestContent()
	{
		return new FormUrlEncodedContent(
		[
			new("client_id", Settings.Value.TwitchAppSettings.ClientId),
			new("client_secret", Settings.Value.TwitchAppSettings.ClientSecret),
			new("grant_type", GrantType.RefreshToken.ConvertToString()),
			new("refresh_token", Settings.Value.StreamerTokens.RefreshToken)
		]);
	}

	public void SaveTokens(ClientCredentialsTokenResponse response)
	{
		Settings.Value.StreamerTokens.SetExpirationTime(response.ExpiresIn);
		Settings.Value.StreamerTokens.AccessToken = response.AccessToken;
		Settings.Value.StreamerTokens.RefreshToken = response.RefreshToken;
	}

	public void SaveSettingsToJson()
	{
		var wrapper = new { AppSettings = Settings.Value };
		wrapper.AppSettings.TwitchEventSubSessionId = "";
		var updatedSettings = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
		File.WriteAllText("ASettings/settings.json", updatedSettings);
	}
}