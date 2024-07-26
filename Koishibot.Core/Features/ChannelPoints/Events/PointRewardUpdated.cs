//using Koishibot.Core.Features.ChannelPoints.Interfaces;
//using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;

//namespace Koishibot.Core.Features.ChannelPoints.Events;

//// == ⚫ EVENT SUB == //

//public class PointRewardUpdated(
//	IOptions<Settings> Settings,
//	EventSubWebsocketClient EventSubClient,
//	ITwitchAPI TwitchApi,
//	IServiceScopeFactory ScopeFactory
//		) : IPointRewardUpdated
//{
//	public async Task SetupHandler()
//	{
//		EventSubClient.ChannelPointsCustomRewardUpdate
//				+= OnPointRewardUpdated;
//		await SubToEvent();
//	}

//	public async Task SubToEvent()
//	{
//		await TwitchApi.CreateEventSubBroadcaster
//				("channel.channel_points_custom_reward.update", "1", Settings);
//	}

//	private async Task OnPointRewardUpdated
//		(object sender, ChannelPointsCustomRewardArgs args)
//	{
//		using var scope = ScopeFactory.CreateScope();
//		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

//		await mediatr.Send(new PointRewardCreatedCommand(args));
//	}
//}

//// == ⚫ COMMAND == //

//public record PointRewardUpdatedCommand
//	(ChannelPointsCustomRewardArgs args) : IRequest;

//// == ⚫ HANDLER == //

///// <summary>
///// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardupdate">ChannelPoints Custom Reward Update EventSub</see></para>
///// </summary>
///// <param name="sender"></param>
///// <param name="e"></param>
//public record PointRewardUpdatedHandler(
//		ILogger<PointRewardUpdatedHandler> Log
//	) : IRequestHandler<PointRewardUpdatedCommand>
//{
//	public async Task Handle
//		(PointRewardUpdatedCommand command, CancellationToken cancellationToken)
//	{
//		Log.LogInformation($"'{command.args.Notification.Payload.Event.Title}' was created");
//		await Task.CompletedTask;
//	}
//}