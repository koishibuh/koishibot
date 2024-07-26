//using Koishibot.Core.Features.ChannelPoints.Interfaces;
//using Koishibot.Core.Features.TwitchUsers.Interfaces;
//using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;

//namespace Koishibot.Core.Features.ChannelPoints.Events;


//// == ⚫ EVENT SUB == //

//public record RewardRedeemUpdated(
//	IOptions<Settings> Settings,
//	EventSubWebsocketClient EventSubClient,
//	ITwitchAPI TwitchApi,
//		IServiceScopeFactory ScopeFactory
//	) : IRewardRedeemUpdated
//{
//	public async Task SetupHandler()
//	{
//		EventSubClient.ChannelPointsCustomRewardRedemptionUpdate
//				+= OnRedeemedRewardUpdated;
//		await SubToEvent();
//	}
//	public async Task SubToEvent()
//	{
//		await TwitchApi.CreateEventSubBroadcaster
//				("channel.channel_points_custom_reward_redemption.update", "1", Settings);
//	}

//	private async Task OnRedeemedRewardUpdated
//		(object sender, ChannelPointsCustomRewardRedemptionArgs args)
//	{
//		using var scope = ScopeFactory.CreateScope();
//		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

//		await mediatr.Send(new RedeemedRewardUpdatedCommand(args));
//	}

//}

//// == ⚫ COMMAND == //

//public record RedeemedRewardUpdatedCommand
//	(ChannelPointsCustomRewardRedemptionArgs args) : IRequest;


//// == ⚫ HANDLER == //

///// <summary>
///// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_reward_redemptionupdate">ChannelPoints Custom Reward Redeem Updated EventSub</see></para>
///// </summary>
///// <param name="sender"></param>
///// <param name="e"></param>
//public record RewardRedeemUpdatedHandler(
//		ITwitchUserHub TwitchUserHub,
//		ILogger<RewardRedeemUpdatedHandler> Log
//	) : IRequestHandler<RedeemedRewardUpdatedCommand>
//{
//	public async Task Handle
//		(RedeemedRewardUpdatedCommand command, CancellationToken cancel)
//	{
//		Log.LogInformation($"OnChannelPointsRedeemUpdated");
//		await Task.CompletedTask;
//	}
//}