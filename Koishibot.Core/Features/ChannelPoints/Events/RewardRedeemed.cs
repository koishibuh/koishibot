using Koishibot.Core.Features.ChannelPoints.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.TwitchEventSub.Extensions;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
namespace Koishibot.Core.Features.ChannelPoints.Events;

// == ⚫ EVENT SUB == //

public record RewardRedeemed(
	IOptions<Settings> Settings,
	EventSubWebsocketClient EventSubClient,
	ITwitchAPI TwitchApi,
	IServiceScopeFactory ScopeFactory
		) : IRewardRedeemed
{
	public async Task SetupHandler()
	{
		EventSubClient.ChannelPointsCustomRewardRedemptionAdd
				+= OnRedeemedReward;
		await SubToEvent();
	}
	public async Task SubToEvent()
	{
		await TwitchApi.CreateEventSubBroadcaster
				("channel.channel_points_custom_reward_redemption.add", "1", Settings);
	}

	private async Task OnRedeemedReward
		(object sender, ChannelPointsCustomRewardRedemptionArgs args)
	{
		using var scope = ScopeFactory.CreateScope();
		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

		await mediatr.Send(new RedeemedRewardCommand(args));
	}
}

// == ⚫ COMMAND == //

public record RedeemedRewardCommand
	(ChannelPointsCustomRewardRedemptionArgs args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_reward_redemptionadd">ChannelPoints Custom Reward Redeemed EventSub</see></para>
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public record RewardRedeemedHandler(
		ITwitchUserHub TwitchUserHub,
		IDragonEggQuestService DragonEggQuestService
	) : IRequestHandler<RedeemedRewardCommand>
{
	public async Task Handle
		(RedeemedRewardCommand command, CancellationToken cancellationToken)
	{
		var e = command.args.ConvertToChannelPointRedeemedEvent();

		var user = await TwitchUserHub.Start(e.User);

		if (e.RewardTitle == "Dragon Egg Quest")
		{
			await DragonEggQuestService.GetResult(user, e.RedeemedAt);
		}
	}

}

public record RedeemedRewardEvent(
				TwitchUserDto User,
				string Message,
				DateTimeOffset RedeemedAt,
				string RewardTitle,
				string RewardId,
				string RewardDescription,
				string RewardFullfillmentStatus,
				int RewardCost
				 );
