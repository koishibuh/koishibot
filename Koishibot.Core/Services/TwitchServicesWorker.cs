//using Koishibot.Core.Features.Application;
//using Koishibot.Core.Features.AttendanceLog.Extensions;
//using Microsoft.Extensions.Hosting;
//namespace Koishibot.Core.Services;

//public class TwitchServicesWorker : BackgroundService
//{
//	private readonly IHostApplicationLifetime _appLifetime;
//	private readonly IAppCache _appCache;
//	private readonly IOptions<Settings> _settings;
//	private readonly IServiceScopeFactory _scopeFactory;
//	private readonly IRefreshAccessTokenService _refreshOAuthTokensService;
//	//private readonly ITwitchEventSubHub _twitchEventSubs;
//	//private readonly IBotIrcHub _botIrcHub;
//	//private readonly IStreamerIrcHub _streamerIrcHub;

//	public TwitchServicesWorker(
//		IHostApplicationLifetime appLifetime,
//		ITwitchEventSubHub twitchEventSubs,
//		IAppCache AppCache,
//		IOptions<Settings> Settings,
//		IServiceScopeFactory ScopeFactory,
//		IRefreshAccessTokenService refreshOAuthTokensService
//		//IBotIrcHub BotIrcHub, IStreamerIrcHub streamerIrcHub
//		)
//	{
//		_appLifetime = appLifetime;
//		_appCache = AppCache;
//		_settings = Settings;
//		_scopeFactory = ScopeFactory;
//		_refreshOAuthTokensService = refreshOAuthTokensService;
//		//_twitchEventSubs = twitchEventSubs;
//		//_botIrcHub = BotIrcHub;
//		//_streamerIrcHub = streamerIrcHub;
//	}

//	protected override Task ExecuteAsync(CancellationToken cancel)
//	{
//		_appLifetime.ApplicationStarted.Register(Start);
//		return Task.CompletedTask;

//		async void Start() => await OnStarted(cancel);
//	}

//	public override Task StopAsync(CancellationToken cancel)
//	{
//		return base.StopAsync(cancel);
//	}

//	private async Task OnStarted(CancellationToken cancel)
//	{
//		_appCache.InitializeServices();
//		_appCache.CreateAttendanceCache();

//		if (_settings.Value.DebugMode is true)
//		{
//			await _refreshOAuthTokensService.EnsureValidOnStartup();

//			{
//				using var scope = _scopeFactory.CreateScope();
//				var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

//				await mediator.Send(new StartupTwitchServicesCommand());
//			}

//			// The application has fully started, start the background tasks
//			//await _refreshOAuthTokensService.EnsureValidOnStartup();

//			//await Task.WhenAll(
//			//	_twitchEventSubs.Start(),
//			//	_streamerIrcHub.Start(),
//			//	_botIrcHub.Start()			
//			//	);

//			//var streamOnline = await _streamInfoService.IsStreamOnline();
//			//if (streamOnline is true)
//			//{
//			//	await _streamOnlineService.Start();
//			//}

//		}
//	}
//}
