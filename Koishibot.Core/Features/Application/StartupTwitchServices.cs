//using Koishibot.Core.Services.Twitch.Irc.Interfaces;

//namespace Koishibot.Core.Features.Application;

//// == ⚫ COMMAND == //

//public record StartupTwitchServicesCommand() : IRequest;

//// == ⚫ HANDLER == //

//public record StartupTwitchServices(
//	//ITwitchEventSubHub TwitchEventSubHub,
//	IServiceScopeFactory ScopeFactory,
//	 ITwitchIrcService Irc
//	//IStreamOnlineApi StreamOnlineApi
//) : IRequestHandler<StartupTwitchServicesCommand>
//{
//	public async Task Handle(StartupTwitchServicesCommand command,
//		CancellationToken cancel)
//	{
//		//await Task.WhenAll(
//		//	TwitchEventSubHub.Start(),
//		//	StreamerIrc.Start(),
//		//	BotIrc.Start()
//		//	);

//		//var result = await StreamOnlineApi.IsStreamOnline();
//		//if (result is true)
//		//{
//		//	using var scope = ScopeFactory.CreateScope();
//		//	var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

//		//	await mediator.Publish(new StreamReconnectCommand());
//		//};
//	}
//}

//// == ⚫ TWITCH API == //

////public record StreamOnlineApi(
////	ILogger<StreamOnlineApi> Log, IOptions<Settings> Settings,
////	ITwitchAPI TwitchApi,
////	IRefreshAccessTokenService TokenProcessor
////	) : IStreamOnlineApi
////{
////	public string StreamerId => Settings.Value.StreamerTokens.UserId;

////	/// <summary>
////	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-streams">Get Streams Documentation</see><br/>
////	/// Id, UserId, UserLogin, UserName, GameId, GameName <br/>
////	/// Type, Title, Tags, ViewerCount, StartedAt, Language<br/>
////	/// ThumbnailUrl, TagIds, IsMature<br/>
////	/// If stream is offline, streams will return null.
////	/// </summary>
////	/// <returns></returns>
////	public async Task<bool> IsStreamOnline()
////	{
////		await TokenProcessor.EnsureValidToken();
////		var streamerIds = new List<string> { StreamerId };

////		var result = await TwitchApi.Helix.Streams.GetStreamsAsync(first: 1, userIds: streamerIds);
////		if (result is null || result.Streams.Length == 0)
////		{
////			Log.LogError("Stream is offline");
////			return false;
////		}

////		return true;
////	}
////}