using Koishibot.Core.Features.Lights;
using Koishibot.Core.Services.OBS;
using Koishibot.Core.Services.StreamElements;
using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.EventSubs;
using Koishibot.Core.Services.Twitch.Irc;
using Microsoft.Extensions.Configuration;
using Todoist.Net;

namespace Koishibot.Core.Configurations;

public static class InfrastructureServiceCollection
{
	public static IServiceCollection AddInfrastructureServices
		(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSingleton<IObsService, ObsService>();
		services.AddSingleton<ISignalrService, SignalrService>();

		services.AddSingleton<ITwitchIrcService, TwitchIrcService>();
		services.AddSingleton<ITwitchEventSubService, TwitchEventSubService>();
		services.AddSingleton<ITwitchApiClient, TwitchApiClient>();
		services.AddSingleton<IStreamElementsService, StreamElementsService>();
		services.AddSingleton<ILightService, LightService>();


		//services.AddTransient<ITodoistService, TodoistService>();
		//services.AddSingleton<IGoogleService, GoogleService>();
		services.AddSingleton<ITodoistClient, TodoistClient>(s =>
		{
			var token = s.GetRequiredService<IOptions<Settings>>().Value.TodoistAccessToken;
			var todoist = new TodoistClient(token);

			return todoist;
		});

		return services;
	}
}