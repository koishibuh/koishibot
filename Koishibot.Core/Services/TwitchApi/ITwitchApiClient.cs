namespace Koishibot.Core.Services.TwitchApi;

public interface ITwitchApiClient
{
	Task<string> SendRequest(HttpMethod httpMethod, string endPoint);
	Task<string> SendRequest(HttpMethod method, string endPoint, string query);
	Task<string> SendRequest(HttpMethod method, string endPoint, string query, StringContent requestBody);
	Task<string> SendRequest(HttpMethod method, string endPoint, StringContent requestBody);
	Task<string> SendRequest(EndPoint endPoint, string query);
	Task<string> SendRequest(EndPoint endPoint, string query, StringContent requestBody);
	Task<string> GetRequest(string endpoint, Dictionary<string, string> queryParams);
	Task<string> PatchRequest<T>(ApiRequest apiType, T requestBody);
		}