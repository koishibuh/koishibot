using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.StreamInformation;
using Koishibot.Core.Features.StreamInformation.Extensions;
using Koishibot.Core.Features.TwitchAuthorization;
using Koishibot.Core.Services.OBS;
using Koishibot.Core.Services.StreamElements;
using Koishibot.Core.Services.Twitch.EventSubs;
using Koishibot.Core.Services.Twitch.Irc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
namespace Koishibot.Core.Services;

/*═══════════════════【 SERVICE 】═══════════════════*/
public class KoishibotBackgroundWorker(
IHostApplicationLifetime appLifetime,
IAppCache appCache,
IOptions<Settings> settings,
IRefreshAccessTokenService refreshOAuthTokensService,
ISignalrService signalR,
ITwitchIrcService twitchIrcService,
ITwitchEventSubService twitchEventSubService,
IObsService obsService,
IStreamElementsService streamElementsService,
IStartupTwitchServices startupTwitchServices,
IStreamStatsService streamStatsService,
[FromKeyedServices("notifications")]HubConnection signalrHub
) : BackgroundService
{
	protected override Task ExecuteAsync(CancellationToken cancel)
	{
		appLifetime.ApplicationStarted.Register(Start);
		return Task.CompletedTask;

		async void Start() => await OnStarted(cancel);
	}

	/*═════════◣ START ◢═════════*/
	private async Task OnStarted(CancellationToken cancel)
	{
		appCache.InitializeServiceStatusCache();
		appCache.CreateAttendanceCache();
		appCache.CreateCommandCache();
		await appCache.LoadCommandCache();
		appCache.CreateTimer();

		await signalrHub.StartAsync(cancel);

		twitchIrcService.SetCancellationToken(cancel);
		twitchEventSubService.SetCancellationToken(cancel);
		streamElementsService.SetCancellationToken(cancel);
		obsService.SetCancellationToken(cancel);

		await refreshOAuthTokensService.Initalize();
		if (settings.Value.StreamerTokens.AccessToken is not "")
		{
			await refreshOAuthTokensService.Start();
			await startupTwitchServices.Start();
		}

		_ = Task.Run(async () => await TwitchTokenTimer(cancel), cancel);
		_ = Task.Run(async () => await StreamStatTimer(cancel), cancel);

	}

	/*═════════◣ TWITCH TOKEN TIMER ◢═════════*/
	private async Task TwitchTokenTimer(CancellationToken cancel)
	{
		{
			var delay = TimeSpan.FromHours(1);
			while (true)
			{
				if (cancel.IsCancellationRequested) { break; }

				var refreshToken = settings.Value.StreamerTokens.RefreshToken;
				try
				{
					if (refreshToken is null) { return;}
					await refreshOAuthTokensService.Start();
				}
				catch (Exception)
				{
					await signalR.SendError("Unable to refresh token");
				}

				await Task.Delay(delay, cancel);
			}
		}
	}

	/*═════════◣ STREAM STAT TIMER ◢═════════*/
	private async Task StreamStatTimer(CancellationToken cancel)
	{
		{
			var delay = TimeSpan.FromMinutes(5);
			while (true)
			{
				if (cancel.IsCancellationRequested) { break; }

				var streamOnline = appCache.StreamOnline();
				try
				{
					if (streamOnline is false) { return; }
					await streamStatsService.Start();
				}
				catch (Exception)
				{
					await signalR.SendError("Unable to fetch stream stats");
				}

				await Task.Delay(delay, cancel);
			}
		}
	}

	/*═════════◣ STOP ◢═════════*/
	public override Task StopAsync(CancellationToken cancel)
		=> base.StopAsync(cancel);
}