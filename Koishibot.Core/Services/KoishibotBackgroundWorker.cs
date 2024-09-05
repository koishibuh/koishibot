using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.StreamInformation.Extensions;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Features.TwitchAuthorization;
using Koishibot.Core.Persistence;
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
[FromKeyedServices("notifications")]HubConnection signalrHub,
IServiceScopeFactory scopeFactory
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
		appCache.CreateTimer();

		using var scope = scopeFactory.CreateScope();
		var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();
		var lastStream = await database.GetLastStream();
		var lastMandatoryStreamDate = await database.GetLastMandatoryStreamDate();
		var streamSessions = new StreamSessions(lastStream, lastMandatoryStreamDate);

		appCache.AddStreamSessions(streamSessions);

		await signalrHub.StartAsync(cancel);

		twitchIrcService.SetCancellationToken(cancel);
		twitchEventSubService.SetCancellationToken(cancel);
		streamElementsService.SetCancellationToken(cancel);
		obsService.SetCancellationToken(cancel);

		if (settings.Value.DebugMode)
		{
			if (settings.Value.StreamerTokens.HasTokenExpired())
			{
				await refreshOAuthTokensService.Start();
			}

			await startupTwitchServices.Start();
		}

		_ = Task.Run(async () =>
		{
			var milliseconds = TimeSpan.FromHours(1);
			while (true)
			{
				if (cancel.IsCancellationRequested) { break; }

				var refreshToken = settings.Value.StreamerTokens.RefreshToken;
				try
				{
					if (refreshToken is null) { return; }
					await refreshOAuthTokensService.Start();
				}
				catch (Exception)
				{
					await signalR.SendError("Unable to refresh token");
				}

				await Task.Delay(milliseconds, cancel);
			}
		}, cancel);
	}

	/*═════════◣ STOP ◢═════════*/
	public override Task StopAsync(CancellationToken cancel)
		=> base.StopAsync(cancel);
}