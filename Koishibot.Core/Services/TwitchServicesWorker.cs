using Koishibot.Core.Features.AttendanceLog.Extensions;
using Microsoft.Extensions.Hosting;
namespace Koishibot.Core.Services;

public class TwitchServicesWorker : BackgroundService
{
	private readonly IHostApplicationLifetime _appLifetime;
	private readonly IAppCache _appCache;
	private readonly IOptions<Settings> _settings;
	private readonly IServiceScopeFactory _scopeFactory;
	private readonly IRefreshAccessTokenService _refreshOAuthTokensService;

	public TwitchServicesWorker(
		IHostApplicationLifetime appLifetime,
		IAppCache AppCache,
		IOptions<Settings> Settings,
		IServiceScopeFactory ScopeFactory,
		IRefreshAccessTokenService refreshOAuthTokensService
		)
	{
		_appLifetime = appLifetime;
		_appCache = AppCache;
		_settings = Settings;
		_scopeFactory = ScopeFactory;
		_refreshOAuthTokensService = refreshOAuthTokensService;
	}

	protected override Task ExecuteAsync(CancellationToken cancel)
	{
		_appLifetime.ApplicationStarted.Register(Start);
		return Task.CompletedTask;

		async void Start() => await OnStarted(cancel);
	}

	public override Task StopAsync(CancellationToken cancel)
	{
		return base.StopAsync(cancel);
	}

	private async Task OnStarted(CancellationToken cancel)
	{
		_appCache.InitializeServices();
		_appCache.CreateAttendanceCache();

		//if (_settings.Value.DebugMode is true)
		//{
		//	await _refreshOAuthTokensService.EnsureValidOnStartup();

		//	{
		//		using var scope = _scopeFactory.CreateScope();
		//		var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

		//		await mediator.Send(new StartupTwitchServicesCommand());
		//	}

		//	// The application has fully started, start the background tasks
		//	//await _refreshOAuthTokensService.EnsureValidOnStartup();

		//	//await Task.WhenAll(
		//	//	_twitchEventSubs.Start(),
		//	//	_streamerIrcHub.Start(),
		//	//	_botIrcHub.Start()			
		//	//	);

		//	//var streamOnline = await _streamInfoService.IsStreamOnline();
		//	//if (streamOnline is true)
		//	//{
		//	//	await _streamOnlineService.Start();
		//	//}

		//}
	}
}
