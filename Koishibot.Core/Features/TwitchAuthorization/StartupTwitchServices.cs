using Koishibot.Core.Features.Obs;
using Koishibot.Core.Features.StreamInformation;
using Koishibot.Core.Services.StreamElements;
using Koishibot.Core.Services.Twitch.EventSubs;
using Koishibot.Core.Services.Twitch.Irc;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.TwitchAuthorization;

/*═══════════════════【 SERVICE 】═══════════════════*/
public record StartupTwitchServices(
IOptions<Settings> Settings,
ITwitchEventSubService TwitchEventSubService,
ITwitchIrcService TwitchIrcService,
ITwitchApiRequest TwitchApiRequest,
IStreamElementsService StreamElementsService,
IObsService ObsService,
IServiceScopeFactory ScopeFactory,
ILogger<StartupTwitchServices> Log
) : IStartupTwitchServices
{
	public async Task Start()
	{
		await TwitchIrcService.CreateWebSocket();
		await TwitchEventSubService.CreateWebSocket();
		// await StreamElementsService.CreateWebSocket();
		await ObsService.CreateWebSocket();

		if (await StreamIsOnline())
		{
			try
			{
				await ReconnectStreamServices();
			}
			catch (Exception ex)
			{
				Log.LogInformation($"{ex}");
			}
		}
	}

/*═════════◣  ◢═════════*/
	private async Task<bool> StreamIsOnline()
	{
		var parameters = new GetLiveStreamsRequestParameters
		{
			UserIds = new List<string> { Settings.Value.StreamerTokens.UserId},
			First = 1
		};

		var listStreamResponse = await TwitchApiRequest.GetLiveStreams(parameters);
		return listStreamResponse.Data?.Count == 1;
	}

	private async Task ReconnectStreamServices()
	{
		using var scope = ScopeFactory.CreateScope();
		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

		await mediatr.Publish(new StreamReconnectCommand());
	}
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IStartupTwitchServices
{
	Task Start();
}