using Newtonsoft.Json;
namespace Koishibot.Core.Features.TwitchAuthorization.Models;

public class ClientCredentialsTokenResponse
{
	[JsonProperty("access_token")]
	public string AccessToken { get; set; }

	[JsonProperty("refresh_token")]
	public string RefreshToken { get; set; }

	[JsonProperty("expires_in")]
	public int ExpiresIn { get; set; }

	[JsonProperty("scope")]
	public List<string> Scopes { get; set; }

	[JsonProperty("token_type")]
	public string TokenType { get; set; }

	//[JsonProperty("id_token")]
	//public string IdToken { get; set; }

}