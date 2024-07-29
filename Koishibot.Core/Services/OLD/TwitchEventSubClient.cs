//using Koishibot.Core.Services.Twitch.EventSubs.RequestModels;
//using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels;
//using Koishibot.Core.Services.TwitchEventSubNew.Enums;
//using System.Net.Http.Headers;
//using System.Net.Http.Json;
//using System.Text.Json;
//namespace Koishibot.Core.Services.Twitch.EventSubs;

//public record TwitchEventSubClient(
//		IOptions<Settings> Settings,
//		IHttpClientFactory HttpClientFactory,
//		ILogger<TwitchEventSubClient> Log
//		) : ITwitchEventSubClient
//{
//	public async Task SubscribeToEvents(List<SubscriptionType> events, string sessionId)
//	{
//		var httpClient = CreateClient();

//		var requests = events.Select
//				(x => CreateEventSubRequest(x, sessionId)).ToList();

//		var eventSubRequestTasks = requests.Select
//				(request => RequestEventSubNotifications(httpClient, request)).ToList();

//		await Task.WhenAll(eventSubRequestTasks);
//	}

//	public CreateEventSubSubscriptionRequestBody CreateEventSubRequest
//			(SubscriptionType type, string sessionId)
//	{
//		var streamerId = Settings.Value.StreamerTokens.UserId;

//		return new CreateEventSubSubscriptionRequestBody
//		{
//			Type = type,
//			Version = EventSubHelper.GetVersionForType(type),
//			Condition = EventSubHelper.GetConditionsForType(type, streamerId),
//			Transport = new EventSubTransportRequest { SessionId = sessionId },
//		};
//	}

//	public async Task<TwitchApiResponse<EventSubResponse>?> RequestEventSubNotifications
//			(HttpClient httpClient, CreateEventSubSubscriptionRequestBody request)
//	{
//		var cancel = new CancellationToken();

//		var uri = new Uri("eventsub/subscriptions", UriKind.Relative);
//		using var response = await httpClient.PostAsJsonAsync(uri, request, cancel);

//		var content = await response.Content.ReadAsStringAsync(cancel);
//		if (!response.IsSuccessStatusCode)
//		{
//			var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
//			// custom exception here
//			throw new Exception($"Failed to subscribe to {request.Type}: {errorResponse?.Message}");
//		}

//		Log.LogInformation($"{request.Type} successful");
//		var result = JsonSerializer.Deserialize<TwitchApiResponse<EventSubResponse>>(content);
//		return result;
//	}

//	public HttpClient CreateClient()
//	{
//		var clientId = Settings.Value.TwitchAppSettings.ClientId;
//		var accessToken = Settings.Value.StreamerTokens.AccessToken;
//		var authenticationHeader = new AuthenticationHeaderValue("Bearer", accessToken);

//		var httpClient = HttpClientFactory.CreateClient("Twitch");
//		httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
//		httpClient.DefaultRequestHeaders.Add("Client-Id", clientId);

//		return httpClient;
//	}
//}

//public interface ITwitchEventSubClient
//{
//	Task SubscribeToEvents(List<SubscriptionType> events, string sessionId);
//}