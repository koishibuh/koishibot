//using Koishibot.Core.Features.TwitchUsers.Interfaces;
//using Koishibot.Core.Persistence;
//using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
//namespace Koishibot.Core.Features.Supports.Events;

//// == ⚫ EVENT SUB == //


//public record PowerUpReceived(
//	IOptions<Settings> Settings,
//	EventSubWebsocketClient EventSubClient,
//	ITwitchAPI TwitchApi,
//	IServiceScopeFactory ScopeFactory
//	) : IPowerUpReceived
//{
//	public async Task SetupMethod()
//	{
//		EventSubClient.ChannelPointsAutomaticRewardRedemptionAdd += OnPowerUpReceived;
//		await SubToEvent();
//	}

//	public async Task SubToEvent()
//	{
//		await TwitchApi.CreateEventSubBroadcaster
//			("channel.channel_points_automatic_reward_redemption.add", "1", Settings);
//	}

//	private async Task OnPowerUpReceived
//		(object sender, ChannelPointsAutomaticRewardRedemptionArgs args)
//	{
//		using var scope = ScopeFactory.CreateScope();
//		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

//		await mediatr.Send(new PowerUpReceivedCommand(args));
//	}
//}

//// == ⚫ COMMAND == //

//public record PowerUpReceivedCommand
//	(ChannelPointsAutomaticRewardRedemptionArgs args) : IRequest;


//// == ⚫ HANDLER == //

///// <summary>
///// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_automatic_reward_redemptionadd""/>Channel Point Auto Reward Redemption Add EventSub Documentation</para>
///// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#channel-points-automatic-reward-redemption-add-event">Event</see></para>
///// </summary>
///// <param name="sender"></param>
///// <param name="e"></param>
//public record PowerUpReceivedHandler(
//	IAppCache Cache, ISignalrService Signalr,
//	ITwitchUserHub TwitchUserHub,
//	KoishibotDbContext Database, ILogger<PowerUpReceivedHandler> Log
//	) : IRequestHandler<PowerUpReceivedCommand>
//{
//	public async Task Handle
//		(PowerUpReceivedCommand command, CancellationToken cancellationToken)
//	{
//		// if type is the following, its a channel point redeem
//		// single_message_bypass_sub_mode
//		// send_highlighted_message
//		// random_sub_emote_unlock
//		// chosen_sub_emote_unlock
//		// chosen_modified_sub_emote_unlock

//		// if type is the following, its a powerup
//		// message_effect
//		// gigantify_an_emote
//		// celebration

//		Log.LogInformation(command.args.Notification.Payload.Event.Id);
//		Log.LogInformation(command.args.Notification.Payload.Event.UserName);
//		Log.LogInformation(command.args.Notification.Payload.Event.Reward.Type);
//		Log.LogInformation(command.args.Notification.Payload.Event.Reward.Cost.ToString());
//	}
//}

//public interface IPowerUpReceived
//{
//	Task SetupMethod();
//	Task SubToEvent();
//}
