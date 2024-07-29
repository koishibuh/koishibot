using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;
namespace Koishibot.Core.Features.Supports.Events;

// == ⚫ COMMAND == //

public record PowerUpReceivedCommand
	(AutomaticRewardRedemptionEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_automatic_reward_redemptionadd""/>Channel Point Auto Reward Redemption Add EventSub Documentation</para>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#channel-points-automatic-reward-redemption-add-event">Event</see></para>
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public record PowerUpReceivedHandler(
	IAppCache Cache, ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database, ILogger<PowerUpReceivedHandler> Log
	) : IRequestHandler<PowerUpReceivedCommand>
{
	public async Task Handle
		(PowerUpReceivedCommand command, CancellationToken cancellationToken)
	{
		// TODO

		// if type is the following, its a channel point redeem
		// single_message_bypass_sub_mode
		// send_highlighted_message
		// random_sub_emote_unlock
		// chosen_sub_emote_unlock
		// chosen_modified_sub_emote_unlock

		// if type is the following, its a powerup
		// message_effect
		// gigantify_an_emote
		// celebration

		Log.LogInformation(command.args.RedemptionId);
		Log.LogInformation(command.args.RedeemedByName);
		Log.LogInformation(command.args.Reward.Type.ToString());
		Log.LogInformation(command.args.Reward.Cost.ToString());
		await Task.CompletedTask;
	}
}