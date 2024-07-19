using Koishibot.Core.Features.ChannelPoints.Interfaces;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;

namespace Koishibot.Core.Features.ChannelPoints.Events;

// == ⚫ EVENT SUB == //

public class PointRewardCreated(
	IOptions<Settings> Settings,
	EventSubWebsocketClient EventSubClient,
	ITwitchAPI TwitchApi,
	IServiceScopeFactory ScopeFactory
	) : IPointRewardCreated
{
	public async Task SetupHandler()
	{
		EventSubClient.ChannelPointsCustomRewardAdd += OnPointRewardCreated;
		await SubToEvent();
	}
	public async Task SubToEvent()
	{
		await TwitchApi.CreateEventSubBroadcaster
			("channel.channel_points_custom_reward.add", "1", Settings);
	}

	private async Task OnPointRewardCreated
		(object sender, ChannelPointsCustomRewardArgs args)
	{
		using var scope = ScopeFactory.CreateScope();
		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

		await mediatr.Send(new PointRewardCreatedCommand(args));
	}
}

// == ⚫ COMMAND == //

public record PointRewardCreatedCommand
	(ChannelPointsCustomRewardArgs args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardadd">ChannelPoints Custom Reward Add EventSub</see></para>
/// Ideally all rewards should be made through the client as they can't be modified otherwise.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public record PointRewardCreatedHandler(
	ILogger<PointRewardCreatedHandler> Log
	) : IRequestHandler<PointRewardCreatedCommand>
{
	public async Task Handle
		(PointRewardCreatedCommand command, CancellationToken cancel)
	{
		Log.LogInformation($"'{command.args.Notification.Payload.Event.Title}' was created");
		await Task.CompletedTask;
	}
}