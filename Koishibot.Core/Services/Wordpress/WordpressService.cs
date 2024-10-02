using Koishibot.Core.Features.Common;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Wordpress.Requests;
using Koishibot.Core.Services.Wordpress.Responses;

namespace Koishibot.Core.Services.Wordpress;

/*═══════════════════【 INTERFACE 】═══════════════════*/
public interface IWordpressService
{
	Task<WordPressResponse> CreateItemTag(string username);
	Task<ItemResponse> CreateItem(AddItemRequest parameters);
	Task<List<GetItemTagResponse>> GetItemTags();
	Task<List<ItemResponse>> GetItems();
	Task<WordPressResponse> GetItemTagById(string wordpressId);
	Task<WordPressResponse?> GetItemTagByName(string username);
}

/*═══════════════════【 SERVICE 】═══════════════════*/
public record WordpressService(
IOptions<Settings> Settings,
IHttpClientFactory HttpClientFactory
) : IWordpressService
{
	private HttpClient CreateClient()
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

		var parameters = new { name = username };

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

	public async Task<ItemResponse> CreateItem(AddItemRequest parameters)
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

		var result = JsonSerializer.Deserialize<ItemResponse>(content);
		if (result is null)
			throw new Exception("Unable to deserialize response");

		return result;
	}

	public async Task<List<GetItemTagResponse>> GetItemTags()
	{
		var httpClient = CreateClient();
		const string endpoint = "item_tag";

		var uriBuilder = new UriBuilder(httpClient.BaseAddress + endpoint);

		var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);
		using var response = await httpClient.SendAsync(request);

		var content = await response.Content.ReadAsStringAsync();
		if (!response.IsSuccessStatusCode)
		{
			var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
			throw new Exception($"{errorResponse?.Message}");
		}

		var result = JsonSerializer.Deserialize<List<GetItemTagResponse>>(content);
		if (result is null)
			throw new Exception("Unable to deserialize response");

		return result;
	}

	public async Task<WordPressResponse> GetItemTagById(string wordpressId)
	{
		var httpClient = CreateClient();
		var endpoint = $"item_tag/{wordpressId}";

		var uriBuilder = new UriBuilder(httpClient.BaseAddress + endpoint);

		var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);
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

	public async Task<WordPressResponse?> GetItemTagByName(string username)
	{
		var httpClient = CreateClient();
		var endpoint = $"item_tag?slug={username}";

		var uriBuilder = new UriBuilder(httpClient.BaseAddress + endpoint);

		var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);
		using var response = await httpClient.SendAsync(request);

		var content = await response.Content.ReadAsStringAsync();
		if (!response.IsSuccessStatusCode)
		{
			var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
			throw new Exception($"{errorResponse?.Message}");
		}

		var result = JsonSerializer.Deserialize<List<WordPressResponse>>(content);
		if (result is null)
			throw new Exception("Unable to deserialize response");

		return result.IsEmpty() ? null : result[0];
	}

	public async Task<List<ItemResponse>> GetItems()
	{
		var httpClient = CreateClient();
		const string endpoint = "items";

		var result = new List<ItemResponse>();

		var pageNumber = 1;
		while (true)
		{
			var response = await GetPageAsync(httpClient, endpoint, pageNumber);

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception($"Error: {response.StatusCode}");
			}

			var pageResult = await DeserializeResponseAsync(response);
			result.AddRange(pageResult);

			var totalPages = int.Parse(response.Headers.GetValues("X-WP-TotalPages").FirstOrDefault());
			if (pageNumber >= totalPages)
			{
				break;
			}

			pageNumber++;
		}

		return result;
	}

	async Task<HttpResponseMessage> GetPageAsync(HttpClient httpClient, string endpoint, int pageNumber)
	{
		var uriBuilder = new UriBuilder(httpClient.BaseAddress + endpoint);
		uriBuilder.Query = $"page={pageNumber}";

		var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);
		return await httpClient.SendAsync(request);
	}

	async Task<List<ItemResponse>> DeserializeResponseAsync(HttpResponseMessage response)
	{
		var content = await response.Content.ReadAsStringAsync();
		return JsonSerializer.Deserialize<List<ItemResponse>>(content);
	}

}