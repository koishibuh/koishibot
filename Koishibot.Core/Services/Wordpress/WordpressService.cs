using System.Net.Http.Headers;
using System.Text;
namespace Koishibot.Core.Services.Wordpress;

public interface IWordpressService
{
}


public record WordpressService(
	IOptions<Settings> Settings,
	IAppCache Cache,
	ISignalrService Signalr,
	IHttpClientFactory HttpClientFactory
	) : IWordpressService
{
	public HttpClient CreateClient()
	{
		var username = Settings.Value.WordpressCredentials.Username;
		var password = Settings.Value.WordpressCredentials.Password;

		var credentials = $"{username}:{password}";

		var bytes = Encoding.ASCII.GetBytes(credentials);
		var base64credentials = Convert.ToBase64String(bytes);
		var authenticationHeader = new AuthenticationHeaderValue("Basic", base64credentials);

		var httpClient = HttpClientFactory.CreateClient("Wordpress");
		httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;

		return httpClient;
	}

	// == ⚫ == //

	//public async Task<string> SendRequest
	//		(HttpMethod httpMethod, string endPoint)
	//{

	//	var httpClient = CreateClient();

	//	var baseUri = httpClient.BaseAddress + endPoint;
	//	var uriBuilder = new UriBuilder(httpClient.BaseAddress + endPoint);

	//	var request = new HttpRequestMessage(httpMethod, uriBuilder.Uri);
	//	using var response = await httpClient.SendAsync(request);

	//	var content = await response.Content.ReadAsStringAsync();
	//	if (!response.IsSuccessStatusCode)
	//	{
	//		var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
	//		// custom exception here
	//		throw new Exception($"Api request failed, {errorResponse?.Message}");
	//	}

	//	return content;
	//}
}

public class ItemResponse
{
	public int Id { get; set; }

	[JsonPropertyName("date_gmt")]
	public DateTimeOffset Date { get; set; }

	public string Status { get; set; }

	public Title Title { get; set; }
}

public class Title
{
	public string Raw { get; set; }
	public string Rendered { get; set; }
}