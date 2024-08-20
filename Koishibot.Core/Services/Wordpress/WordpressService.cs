using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Koishibot.Core.Features.ChannelPoints.Controllers;
using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Wordpress.Requests;
using Koishibot.Core.Services.Wordpress.Responses;

namespace Koishibot.Core.Services.Wordpress;

public interface IWordpressService
{
	Task<WordPressResponse> CreateItemTag(string username);
	Task<AddItemResponse> CreateItem(AddItemRequest parameters);
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
		var base64Credentials = Convert.ToBase64String(bytes);
		var authenticationHeader = new AuthenticationHeaderValue("Basic", base64Credentials);

		var httpClient = HttpClientFactory.CreateClient("Wordpress");
		httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;

		return httpClient;
	}

	public async Task<WordPressResponse> CreateItemTag(string username)
	{
		var httpClient = CreateClient();
		const string endpoint = "item_tag";

		var parameters = new { Name = username };

		var uriBuilder = new UriBuilder(httpClient.BaseAddress + endpoint);
		uriBuilder.Query = parameters.ObjectQueryFormatter();

		var request = new HttpRequestMessage(HttpMethod.Post, uriBuilder.Uri);
		using var response = await httpClient.SendAsync(request);

		var content = await response.Content.ReadAsStringAsync();
		if (!response.IsSuccessStatusCode)
		{
			var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
			throw new Exception($"{errorResponse?.Message}");
		}

		var result = JsonSerializer.Deserialize<WordPressResponse>(content);
		if (result is null)
			throw new Exception("Unable to deserialize response");

		return result;
	}

	public async Task<AddItemResponse> CreateItem(AddItemRequest parameters)
	{
		var httpClient = CreateClient();
		const string endpoint = "items";

		var uriBuilder = new UriBuilder(httpClient.BaseAddress + endpoint);
		uriBuilder.Query = parameters.ObjectQueryFormatter();

		var request = new HttpRequestMessage(HttpMethod.Post, uriBuilder.Uri);
		using var response = await httpClient.SendAsync(request);

		var content = await response.Content.ReadAsStringAsync();
		if (!response.IsSuccessStatusCode)
		{
			var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
			throw new Exception($"{errorResponse?.Message}");
		}

		var result = JsonSerializer.Deserialize<AddItemResponse>(content);
		if (result is null)
			throw new Exception("Unable to deserialize response");

		return result;
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