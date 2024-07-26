using Koishibot.Core.Features.Lights;
using Koishibot.Core.Features.Obs;
using Koishibot.Core.Features.Obs.Interfaces;
using Koishibot.Core.Services.Twitch.EventSubs;
using Koishibot.Core.Services.TwitchApi;
using Microsoft.Extensions.Configuration;
using OBSStudioClient;
using Todoist.Net;
using TwitchLib.Client;

namespace Koishibot.Core.Configurations;

public static class InfrastructureServiceCollection
{
	public static IServiceCollection AddInfrastructureServices
		(this IServiceCollection services, IConfiguration configuration)
	{
		//services.AddSingleton<ITwitchAPI>(s =>
		//{
		//	var twitchApi = new TwitchAPI();
		//	twitchApi.Settings.ClientId =
		//			s.GetRequiredService<IOptions<Settings>>().Value.TwitchAppSettings.ClientId;
		//	return twitchApi;
		//});

		services.AddSingleton(s => new ObsClient());
		services.AddTransient<IObsService, ObsService>();
		
		//services.AddSingleton(s => new StreamerTwitchClient());
		//services.AddSingleton(s => new BotTwitchClient());
		//services.AddTransient<IStreamerIrcHub, StreamerIrcHub>();
		//services.AddTransient<IBotIrcHub, BotIrcHub>();


		services.AddTransient<ISignalrService, SignalrService>();
		
		//services.AddTransient<IChatMessageService, ChatMessageService>();
		//services.AddSingleton<IGoogleService, GoogleService>();

		//services.AddTwitchLibEventSubWebsockets();
		//services.AddSingleton<ITwitchEventSubHub, TwitchEventSubHub>();

		services.AddSingleton<ITwitchEventSubService, TwitchEventSubService>();
		services.AddSingleton<ITwitchEventSubClient, TwitchEventSubClient>();
		services.AddSingleton<ITwitchApiClient, TwitchApiClient>();


		services.AddSingleton<ILightService, LightService>();	

		//services.AddSingleton<ITwitchIrcHub, TwitchIrcHub>();

		//services.AddTransient<ITodoistService, TodoistService>();
		services.AddSingleton<ITodoistClient, TodoistClient>(s =>
		{
			var token = s.GetRequiredService<IOptions<Settings>>().Value.TodoistAccessToken;
			var todoist = new TodoistClient(token);

			return todoist;
		});

		return services;
	}
}

public class StreamerTwitchClient : TwitchClient { }

public class BotTwitchClient : TwitchClient { }