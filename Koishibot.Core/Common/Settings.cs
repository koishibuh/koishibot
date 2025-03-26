namespace Koishibot.Core.Common;

public class Settings
{
	public bool DebugMode { get; set; }
	public TwitchAppSettings TwitchAppSettings { get; set; } = null!;	
	public TwitchTokens StreamerTokens { get; set; } = null!;
	public TwitchTokens BotTokens { get; set;} = null!;
	public ObsSettings ObsSettings { get; set; } = null!;
	public string TwitchEventSubSessionId { get; set; } = string.Empty;
	public string TodoistAccessToken { get; set; } = string.Empty;
	public string StreamElementsJwtToken { get; set; }= string.Empty;
	public string KofiVerificationToken { get; set; } = string.Empty;
	public AppAuthentication AppAuthentication { get; set; } = null!;
	public GoogleTokens GoogleTokens { get; set; } = null!;
	public string MagicHueToken { get; set; } = string.Empty;
	public WordpressCredentials WordpressCredentials { get; set; } = null!;
}

public class AppAuthentication
{
	public string Key { get; set; } = string.Empty;
	public string Issuer { get; set; } = string.Empty;
	public string Audience { get; set; } = string.Empty;
	public string EGKey { get; set; } = string.Empty;
}

public class TwitchAppSettings 
{
	public string ClientId { get; set; } = string.Empty;
	public string ClientSecret { get; set; } = string.Empty;
	public string RedirectUri { get; set; } = string.Empty;

	public string CreateStateVerify()
	{
		return ((long)DateTime.UtcNow
			.Subtract(new DateTime(2017, 1, 7)).TotalSeconds)
			.ToString();
	}
}
public class TwitchTokens
{
	public string UserId { get; set; } = string.Empty;
	public string UserLogin { get; set; } = string.Empty; // lowercase
	public string? AccessToken { get; set; }
	public string? RefreshToken { get; set; }
	public DateTime ExpirationDate { get; set; }

	public bool HasTokenExpired()
	{
		return DateTime.Now >= ExpirationDate;
	}

	public void SetExpirationTime(int expiresInSeconds)
	{
		ExpirationDate = DateTime.UtcNow
			.AddSeconds(expiresInSeconds)
			.Subtract(TimeSpan.FromMinutes(10));
	}
}

public class ObsSettings
{
	public string WebsocketUrl { get; set; } = string.Empty;
	public string Port { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}

public class GoogleTokens
{
	public string ApplicationName { get; set; } = string.Empty;
	public string ClientEmail { get; set; } = string.Empty;
	public string PrivateKey { get; set; } = string.Empty;
	public string TwitchCalendarId { get; set; } = string.Empty;
}

public class WordpressCredentials
{
	public string Username { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}