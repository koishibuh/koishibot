using Koishibot.Core.Features.StreamInformation;
using Koishibot.Core.Features.TwitchAuthorization.Enums;
using Koishibot.Core.Features.TwitchAuthorization.Models;
using Koishibot.Core.Services.Twitch.EventSubs;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
using Koishibot.Core.Services.TwitchApi.Models;
using Newtonsoft.Json;
namespace Koishibot.Core.Features.TwitchAuthorization.Controllers;

public class GetOAuthTokensController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Twitch Oauth"])]
	[HttpPost("/api/twitch-auth/token")]
	public async Task<ActionResult> GetOAuthTokens([FromBody] GetOAuthTokensQuery query)
	{
		await Mediator.Send(query);
		return Ok();
	}
}

public record GetOAuthTokensQuery(string Code) : IRequest;

public record GetOAuthTokensHandler(
	IOptions<Settings> Settings,
	IHttpClientFactory HttpClientFactory,
	ITwitchEventSubService TwitchEventSubService,
	IValidateTokenService ValidateTokenService,
	ITwitchIrcService TwitchIrcService,
	ITwitchApiRequest TwitchApiRequest,
	IServiceScopeFactory ScopeFactory,
	ILogger<GetOAuthTokensHandler> Log
	)
	: IRequestHandler<GetOAuthTokensQuery>
{
	public async Task Handle(GetOAuthTokensQuery query, CancellationToken cancel)
	{
		var requestContent = CreateRequestContent(query.Code);

		var httpClient = HttpClientFactory.CreateClient("Default");
		var uri = new Uri("https://id.twitch.tv/oauth2/token");

		using var response = await httpClient.PostAsync(uri, requestContent);
		var content = await response.Content.ReadAsStringAsync();

		var result = JsonConvert.DeserializeObject<ClientCredentialsTokenResponse>(content)
			?? throw new Exception("An error occurred while generating a twitch token.");

		SaveTokens(result);

		// Validate token
		await ValidateTokenService.Start();

		await TwitchIrcService.CreateWebSocket();
		await TwitchEventSubService.CreateWebSocket();
		
		var liveStreamParameters = new GetLiveStreamsRequestParameters
		{
			UserIds = new List<string> { Settings.Value.StreamerTokens.UserId},
			First = 1
		};
		var listStreamResponse = await TwitchApiRequest.GetLiveStreams(liveStreamParameters);
		if (listStreamResponse.Data.Count == 1)
		{
			try
			{
				using var scope = ScopeFactory.CreateScope();
				var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

				await mediatr.Publish(new StreamReconnectCommand());
			} 
			catch (Exception ex) 
			{
				Log.LogInformation($"{ex}");
			}
		}
		
		//await Task.WhenAll(
		//		TwitchEventSubHub.Start(),
		//		StreamerIrc.Start(),
		//		BotIrc.Start()
		//		);
	}

	private FormUrlEncodedContent CreateRequestContent(string code)
	{
		return new FormUrlEncodedContent(
		[
			new("client_id", Settings.Value.TwitchAppSettings.ClientId),
			new("client_secret", Settings.Value.TwitchAppSettings.ClientSecret),
			new("grant_type", GrantType.AuthorizationCode.ConvertToString()),
			new("code", code),
			new("redirect_uri", Settings.Value.TwitchAppSettings.RedirectUri)
		]);
	}

	private void SaveTokens(ClientCredentialsTokenResponse response)
	{
		Settings.Value.StreamerTokens.SetExpirationTime(response.ExpiresIn);
		Settings.Value.StreamerTokens.AccessToken = response.AccessToken;
		Settings.Value.StreamerTokens.RefreshToken = response.RefreshToken;
	}
}