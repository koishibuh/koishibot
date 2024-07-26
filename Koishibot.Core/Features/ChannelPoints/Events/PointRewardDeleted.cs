//using Koishibot.Core.Features.ChannelPoints.Interfaces;
//using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;

//namespace Koishibot.Core.Features.ChannelPoints.Events;

//// == ⚫ EVENT SUB == //

//public class PointRewardDeleted(
//	IOptions<Settings> Settings,
//	EventSubWebsocketClient EventSubClient,
//	ITwitchAPI TwitchApi,
//	IServiceScopeFactory ScopeFactory
//		) : IPointRewardDeleted
//{
//	public async Task SetupHandler()
//	{
//		EventSubClient.ChannelPointsCustomRewardUpdate
//				+= OnPointRewardDeleted;
//		await SubToEvent();
//	}

//	public async Task SubToEvent()
//	{
//		await TwitchApi.CreateEventSubBroadcaster
//				("channel.channel_points_custom_reward.remove", "1", Settings);
//	}

//	private async Task OnPointRewardDeleted
//		(object sender, ChannelPointsCustomRewardArgs args)
//	{
//		using var scope = ScopeFactory.CreateScope();
//		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

//		await mediatr.Send(new PointRewardCreatedCommand(args));
//	}
//}

//// == ⚫ COMMAND == //

//public record PointRewardDeletedCommand
//	(ChannelPointsCustomRewardArgs args) : IRequest;


//// == ⚫ HANDLER == //

///// <summary>
///// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardremove">ChannelPoints Custom Reward Remove EventSub</see></para>
///// </summary>
///// <param name="sender"></param>
///// <param name="e"></param>
//public record PointRewardDeletedHandler(
//		ILogger<PointRewardUpdatedHandler> Log
//	) : IRequestHandler<PointRewardDeletedCommand>
//{
//	public async Task Handle(PointRewardDeletedCommand command,
//		CancellationToken cancel)
//	{
//		Log.LogInformation($"'{command.args.Notification.Payload.Event.Title}' was created");
//		await Task.CompletedTask;
//	}
//}
