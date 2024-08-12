using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.TwitchApi.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
namespace Koishibot.Core.Services.Twitch;

public record TwitchApiClient(
	ILogger<TwitchApiClient> Log,
		IOptions<Settings> Settings,
		IHttpClientFactory HttpClientFactory,
		IRefreshAccessTokenService RefreshAccessTokenService
		) : ITwitchApiClient
{

	public async Task EnsureValidToken()
	{
		var StreamerTokens = Settings.Value.StreamerTokens;

		var result = StreamerTokens.HasTokenExpired();
		if (result is true)
		{
			await RefreshAccessTokenService.Start();
		}
	}

	public HttpClient CreateClient()
	{

		var clientId = Settings.Value.TwitchAppSettings.ClientId;
		var accessToken = Settings.Value.StreamerTokens.AccessToken;
		var authenticationHeader = new AuthenticationHeaderValue("Bearer", accessToken);

		var httpClient = HttpClientFactory.CreateClient("Twitch");
		httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
		httpClient.DefaultRequestHeaders.Add("Client-Id", clientId);

		return httpClient;
	}

	// == ⚫ REQUESTS == //

	//public async Task<string> SendRequestInternal(HttpMethod httpMethod, Uri uri, HttpContent content = null)
	//{
	//	await EnsureValidToken();

	//	var httpClient = CreateClient();
	//	var request = new HttpRequestMessage(httpMethod, uri) { Content = content };

	//	using var response = await httpClient.SendAsync(request);
	//	var responseContent = await response.Content.ReadAsStringAsync();

	//	if (!response.IsSuccessStatusCode)
	//	{
	//		var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent);
	//		// custom exception here
	//		throw new Exception($"Api request failed, {errorResponse?.Message}");
	//	}

	//	return responseContent;
	//}

	//public async Task<string> SendRequestNew(HttpClient httpClient, HttpMethod httpMethod, Uri uri, HttpContent content = null)
	//{
	//	var request = new HttpRequestMessage(httpMethod, uri) { Content = content };

	//	using var response = await httpClient.SendAsync(request);
	//	var responseContent = await response.Content.ReadAsStringAsync();


	//	if (!response.IsSuccessStatusCode)
	//	{
	//		var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent);
	//		Log.LogInformation($"Api request failed, {errorResponse?.Message}");
	//		// custom exception here
	//		throw new Exception($"Api request failed, {errorResponse?.Message}");
	//	}

	//	return responseContent;
	//}




	//public async Task<string> SendRequest(HttpMethod httpMethod, string endPoint)
	//{
	//	await EnsureValidToken();
	//	var httpClient = CreateClient();

	//	var uri = new Uri(CreateClient().BaseAddress + endPoint);
	//	return await SendRequestNew(httpClient, httpMethod, uri);
	//}

	//public async Task<string> SendRequest(HttpMethod httpMethod, string endPoint, string query)
	//{
	//	await EnsureValidToken();
	//	var httpClient = CreateClient();

	//	var uriBuilder = new UriBuilder(CreateClient().BaseAddress + endPoint) { Query = query };
	//	return await SendRequestNew(httpClient, httpMethod, uriBuilder.Uri);
	//}

	//public async Task<string> SendRequest(HttpMethod httpMethod, string endPoint, StringContent body)
	//{
	//	await EnsureValidToken();
	//	var httpClient = CreateClient();

	//	var uri = new Uri(CreateClient().BaseAddress + endPoint);
	//	return await SendRequestNew(httpClient, httpMethod, uri, body);
	//}

	//public async Task<string> SendRequest(HttpMethod httpMethod, string endPoint, string query, StringContent body)
	//{
	//	await EnsureValidToken();
	//	var httpClient = CreateClient();

	//	var uriBuilder = new UriBuilder(CreateClient().BaseAddress + endPoint) { Query = query };
	//	return await SendRequestNew(httpClient, httpMethod, uriBuilder.Uri, body);
	//}

	//public async Task<List<string>> SendRequest(string endPoint, List<CreateEventSubSubscriptionRequestBody> requestBody)
	//{
	//	await EnsureValidToken();
	//	var httpClient = CreateClient();

	//	var responses = new List<string>();
	//	var uri = new Uri(CreateClient().BaseAddress + endPoint);

	//	foreach (var body in requestBody)
	//	{
	//		var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
	//		responses.Add(await SendRequestNew(httpClient, HttpMethod.Post, uri, content));
	//	}

	//	return responses;
	//}

	public async Task<string> SendRequest
			(HttpMethod httpMethod, string endPoint)
	{
		await EnsureValidToken();

		var httpClient = CreateClient();

		var baseUri = httpClient.BaseAddress + endPoint;
		var uriBuilder = new UriBuilder(httpClient.BaseAddress + endPoint);

		var request = new HttpRequestMessage(httpMethod, uriBuilder.Uri);
		using var response = await httpClient.SendAsync(request);

		var content = await response.Content.ReadAsStringAsync();
		if (!response.IsSuccessStatusCode)
		{
			var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
			// custom exception here
			throw new Exception($"Api request failed, {errorResponse?.Message}");
		}

		return content;
	}


	public async Task<string> SendRequest
			(HttpMethod httpMethod, string endPoint, string query)
	{
		await EnsureValidToken();

		var httpClient = CreateClient();
		var uriBuilder = new UriBuilder(httpClient.BaseAddress + endPoint);
		uriBuilder.Query = query;

		var request = new HttpRequestMessage(httpMethod, uriBuilder.Uri);
		using var response = await httpClient.SendAsync(request);

		var content = await response.Content.ReadAsStringAsync();
		if (!response.IsSuccessStatusCode)
		{
			var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
			// custom exception here
			throw new Exception($"Api request failed, {errorResponse?.Message}");
		}

		return content;
	}

	public async Task<string> SendRequest
			(HttpMethod httpMethod, string endPoint, StringContent body)
	{

		await EnsureValidToken();

		var httpClient = CreateClient();
		var uriBuilder = new UriBuilder(httpClient.BaseAddress + endPoint);

		var request = new HttpRequestMessage(httpMethod, uriBuilder.Uri) { Content = body };
		using var response = await httpClient.SendAsync(request);

		var content = await response.Content.ReadAsStringAsync();
		if (!response.IsSuccessStatusCode)
		{
			var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
			// custom exception here
			throw new Exception($"Api request failed, {errorResponse?.Message}");
		}

		return content;
	}

	public async Task<string> SendRequest
			(HttpMethod httpMethod, string endPoint, string query, StringContent body)
	{
		await EnsureValidToken();

		var httpClient = CreateClient();
		var uriBuilder = new UriBuilder(httpClient.BaseAddress + endPoint);
		uriBuilder.Query = query;

		var request = new HttpRequestMessage(httpMethod, uriBuilder.Uri) { Content = body };
		using var response = await httpClient.SendAsync(request);

		var content = await response.Content.ReadAsStringAsync();
		if (!response.IsSuccessStatusCode)
		{
			var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
			// custom exception here
			throw new Exception($"Api request failed, {errorResponse?.Message}");
		}

		return content;
	}

	public async Task<List<string>> SendRequest
	(string endPoint, List<CreateEventSubSubscriptionRequestBody> requestBody)
	{
		await EnsureValidToken();

		var httpClient = CreateClient();

		var baseUri = httpClient.BaseAddress + endPoint;
		var uriBuilder = new UriBuilder(baseUri);

		var responses = new List<string>();

		foreach (var body in requestBody)
		{
			using var response = await httpClient.PostAsJsonAsync(uriBuilder.Uri, body);

			var content = await response.Content.ReadAsStringAsync();
			if (!response.IsSuccessStatusCode)
			{
				var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
				Log.LogInformation($"Api request failed, {errorResponse?.Message}");
				// custom exception here
				throw new Exception($"Failed to subscribe to event: {errorResponse?.Message}");
			}

			responses.Add(content);
		}

		return responses;
	}
}


public interface ITwitchApiClient
{
	Task<string> SendRequest(HttpMethod httpMethod, string endPoint);
	Task<string> SendRequest(HttpMethod method, string endPoint, string query);
	Task<string> SendRequest(HttpMethod method, string endPoint, string query, StringContent requestBody);
	Task<string> SendRequest(HttpMethod method, string endPoint, StringContent requestBody);
	Task<List<string>> SendRequest(string endPoint, List<CreateEventSubSubscriptionRequestBody> requestBody);
}