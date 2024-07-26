using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Koishibot.Core.Services.TwitchApi;

public record TwitchApiClient(
	IOptions<Settings> Settings,
	IHttpClientFactory HttpClientFactory
	//IRefreshAccessTokenService TokenProcessor
	) : ITwitchApiClient
{
	public async Task<string> SendRequest(HttpMethod httpMethod, string endPoint)
	{
		//await TokenProcessor.EnsureValidToken();

		var httpClient = CreateClient();

		var baseUri = httpClient.BaseAddress + endPoint;
		var uriBuilder = new UriBuilder(baseUri);
	
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


	public async Task<string> SendRequest(HttpMethod httpMethod, string endPoint, string query)
	{
		//await TokenProcessor.EnsureValidToken();

		var httpClient = CreateClient();

		var baseUri = httpClient.BaseAddress + endPoint;
		var uriBuilder = new UriBuilder(baseUri);
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

	public async Task<string> SendRequest(HttpMethod httpMethod, string endPoint, StringContent body)
	{

		//await TokenProcessor.EnsureValidToken();

		var httpClient = CreateClient();

		var baseUri = httpClient.BaseAddress + endPoint;
		var uriBuilder = new UriBuilder(baseUri);

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


	public async Task<string> SendRequest(HttpMethod httpMethod, string endPoint, string query, StringContent body)
	{
		//await TokenProcessor.EnsureValidToken();

		var httpClient = CreateClient();

		var baseUri = httpClient.BaseAddress + endPoint;
		var uriBuilder = new UriBuilder(baseUri);
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


	public async Task<string> SendRequest(EndPoint endPoint, string query)
	{
		//await TokenProcessor.EnsureValidToken();

		var httpClient = CreateClient();

		var baseUri = httpClient.BaseAddress + endPoint.Url;
		var uriBuilder = new UriBuilder(baseUri);
		uriBuilder.Query = query;
		
		var request = new HttpRequestMessage(endPoint.HttpMethod, uriBuilder.Uri);
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


	public async Task<string> SendRequest(EndPoint endPoint, string query, StringContent requestBody)
	{

		//await TokenProcessor.EnsureValidToken();
		var httpClient = CreateClient();

		var baseUri = httpClient.BaseAddress + endPoint.Url;
		var uriBuilder = new UriBuilder(baseUri);
		uriBuilder.Query = query;

		var request = new HttpRequestMessage(endPoint.HttpMethod, uriBuilder.Uri) { Content = requestBody };
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



	public async Task<string> GetRequest(string endpoint, Dictionary<string, string> queryParams)
	{
		//await TokenProcessor.EnsureValidToken();
		var httpClient = CreateClient();
		var baseUri = httpClient.BaseAddress + endpoint;

		var uriBuilder = new UriBuilder(baseUri!);
		//var queryParams = new Dictionary<string, string>
		//{
		//{"param1", "value1"},
		//{"param2", "value2"},
		//{"param3", "value3"}
		//};

		var query =	string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
		uriBuilder.Query = query;

		var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);
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


	public async Task<string> PostRequest<T>(ApiRequest apiType, T requestBody)
	{
		//await TokenProcessor.EnsureValidToken();
		var endpoint = TwitchApiHelper.GetEndpoint(ApiRequest.EditChannelInfo);
		var queryParams = TwitchApiHelper.GetQueryParameters(ApiRequest.EditChannelInfo, Settings);

		var httpClient = CreateClient();
		var baseUri = httpClient.BaseAddress + endpoint.Url;

		var uriBuilder = new UriBuilder(baseUri!);
		var query = string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
		uriBuilder.Query = query;

		var content = ConvertToStringContent(requestBody);
		var request = new HttpRequestMessage(HttpMethod.Post, uriBuilder.Uri) { Content = content };
		using var response = await httpClient.SendAsync(request);

		var responseContent = await response.Content.ReadAsStringAsync();
		if (!response.IsSuccessStatusCode)
		{
			var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent);
			// custom exception here
			throw new Exception($"Api request failed, {errorResponse?.Message}");
		}

		return responseContent;
	}


	public async Task<string> PatchRequest<T>(ApiRequest apiType, T requestBody)
	{
		//await TokenProcessor.EnsureValidToken();
		var endpoint = TwitchApiHelper.GetEndpoint(apiType);
		var queryParams = TwitchApiHelper.GetQueryParameters(apiType, Settings);

		var httpClient = CreateClient();
		var baseUri = httpClient.BaseAddress + endpoint.Url;

		var uriBuilder = new UriBuilder(baseUri!);
		var query = string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
		uriBuilder.Query = query;

		var content = ConvertToStringContent(requestBody);

		var request =	new HttpRequestMessage(HttpMethod.Patch, uriBuilder.Uri) { Content = content };
		using var response = await httpClient.SendAsync(request);

		var responseContent = await response.Content.ReadAsStringAsync();
		if (!response.IsSuccessStatusCode)
		{
			var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent);
			// custom exception here
			throw new Exception($"Api request failed, {errorResponse?.Message}");
		}

		return responseContent;
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

	public StringContent ConvertToStringContent<T>(T requestBody)
	{
		var json = JsonSerializer.Serialize(requestBody);
		var content = new StringContent(json, Encoding.UTF8, "application/json");
		return content;
	}
}
