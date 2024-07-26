//using Newtonsoft.Json;

//namespace Koishibot.Core.Features.TwitchAuthorization;

//public record RefreshAccessTokenService(
//	IOptions<Settings> Settings,
//	//ITwitchAPI TwitchApi,
//	ILogger<RefreshAccessTokenService> Log,
//	IHttpClientFactory HttpClientFactory
//	) : IRefreshAccessTokenService
//{


//	public async Task<bool> StartWithToken(string token)
//	{
//		try
//		{
//			var clientSecret = Settings.Value.TwitchAppSettings.ClientSecret;

//			var result = await TwitchApi.Auth.RefreshAuthTokenAsync(token, clientSecret);

//			if (result is null)
//			{
//				// return bad request
//				return false;
//			}

//			TwitchApi.Settings.AccessToken = result.AccessToken;
//			Settings.Value.StreamerTokens.SetExpirationTime(result.ExpiresIn);
//			Settings.Value.StreamerTokens.AccessToken = result.AccessToken;
//			Settings.Value.StreamerTokens.RefreshToken = result.RefreshToken;

//#if DEBUG
//			SaveSettingsToJson();
//#endif

//			return true;
//		}
//		catch (Exception ex)
//		{
//			Log.LogError(ex, "Unable to refresh Oauth Tokens");
//			return false;
//		}
//	}


//	public async Task<bool> Start()
//	{
//		try
//		{
//			var clientSecret = Settings.Value.TwitchAppSettings.ClientSecret;
//			var refreshToken = Settings.Value.StreamerTokens.RefreshToken;

//			var result = await TwitchApi.Auth.RefreshAuthTokenAsync(refreshToken, clientSecret);

//			if (result is null)
//			{
//				// return bad request
//				return false;
//			}

//			TwitchApi.Settings.AccessToken = result.AccessToken;
//			Settings.Value.StreamerTokens.SetExpirationTime(result.ExpiresIn);
//			Settings.Value.StreamerTokens.AccessToken = result.AccessToken;
//			Settings.Value.StreamerTokens.RefreshToken = result.RefreshToken;

//#if DEBUG
//			SaveSettingsToJson();
//#endif

//			return true;
//		}
//		catch (Exception ex)
//		{
//			Log.LogError(ex, "Unable to refresh Oauth Tokens");
//			return false;
//		}

//	}

//	public void SaveSettingsToJson()
//	{
//		var wrapper = new { AppSettings = Settings.Value };
//		wrapper.AppSettings.TwitchEventSubSessionId = "";
//		var updatedSettings = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
//		File.WriteAllText("ASettings/settings.json", updatedSettings);
//	}

//	public async Task EnsureValidToken()
//	{
//		//Log.LogInformation("Checking token");
//		var StreamerTokens = Settings.Value.StreamerTokens;

//		var result = StreamerTokens.HasTokenExpired();
//		if (result is true)
//		{
//			Log.LogInformation("Getting a new refresh token");
//			await Start();
//		}
//	}

//	public async Task EnsureValidOnStartup()
//	{
//		var StreamerTokens = Settings.Value.StreamerTokens;
//		var result = StreamerTokens.HasTokenExpired();
//		if (result is true)
//		{
//			Log.LogInformation("Getting a new refresh token");
//			await Start();
//		}
//		else
//		{
//			TwitchApi.Settings.AccessToken = StreamerTokens.AccessToken;
//		}
//	}
//}