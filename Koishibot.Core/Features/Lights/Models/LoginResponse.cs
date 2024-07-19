using Newtonsoft.Json;

namespace Koishibot.Core.Features.Lights.Models;

public class LoginResponse
{
	[JsonProperty("token")]
	public string Token { get; set; } = string.Empty;
}
