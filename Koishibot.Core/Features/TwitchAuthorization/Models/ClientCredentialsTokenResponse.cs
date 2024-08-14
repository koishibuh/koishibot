namespace Koishibot.Core.Features.TwitchAuthorization.Models;

public class ClientCredentialsTokenResponse
{
	[JsonPropertyName("access_token")]
	public string? AccessToken { get; init; }

	[JsonPropertyName("refresh_token")]
	public string? RefreshToken { get; init; }

	[JsonPropertyName("expires_in")]
	public int ExpiresIn { get; init; }

	[JsonPropertyName("scope")]
	public List<string>? Scopes { get; init; }

	[JsonPropertyName("token_type")]
	public string? TokenType { get; init; }

	//[JsonProperty("id_token")]
	//public string IdToken { get; set; }
}