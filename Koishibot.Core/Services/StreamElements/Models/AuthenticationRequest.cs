using System.Text.Json;
namespace Koishibot.Core.Services.StreamElements.Models;

public class AuthenticationRequest
{
	public string Event { get; set; } = "authenticate";
	public AuthenticationData Data { get; set; }

	public AuthenticationRequest(string jwt)
	{
		Data = new AuthenticationData
		{
			Token = jwt
		};
	}

	public string ConvertToString()
	{
		var json = JsonSerializer.Serialize(new object[] { Event, Data });
		return $"42{json}";
	}
}

public class AuthenticationData
{
	[JsonPropertyName("method")]
	public string Method { get; set; } = "jwt";
	[JsonPropertyName("token")]
	public string Token { get; set; } = null!;
}