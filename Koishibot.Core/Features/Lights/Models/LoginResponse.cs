using Newtonsoft.Json;

namespace Koishibot.Core.Features.Lights.Models;

public class LoginResponse
{
	[JsonProperty("code")]
	public string Code { get; set; } = string.Empty;

	[JsonProperty("msg")]
	public string Message { get; set;} = string.Empty;

	[JsonProperty("token")]
	public string Token { get; set; } = string.Empty;
}
