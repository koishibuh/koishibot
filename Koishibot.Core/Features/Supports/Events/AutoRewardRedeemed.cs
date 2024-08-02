using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Supports.Extensions;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;
namespace Koishibot.Core.Features.Supports.Events;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_automatic_reward_redemptionadd""/>Channel Point Auto Reward Redemption Add EventSub Documentation</para>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#channel-points-automatic-reward-redemption-add-event">Event</see></para><br/>
/// PowerUps: MessageEffect, GigantifyAnEmote, Celebration</br>
/// PointRedeems: BypassSubOnly, HighlightedMessage, RandomSubEmote, ChosenSubEmote, ChosenModifySubEmote 
/// </summary>
public record AutoRewardRedeemedHandler(
	IAppCache Cache, ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<AutoRewardRedeemedCommand>
{
	public async Task Handle
		(AutoRewardRedeemedCommand command, CancellationToken cancellationToken)
	{
		var userDto = command.CreateUserDto();
		var user = await TwitchUserHub.Start(userDto);

		if (command.RedeemIsPowerUp())
		{
			var cheer = command.CreateCheer(user.Id);
			await Database.UpdateEntry(cheer);
			await Database.UpdateCheerTotal(cheer);

			var vm = cheer.CreateVm(user.Name);
			await Signalr.SendStreamEvent(vm);

			// TODO: Do thing to chat message?
		}
		else
		{
			// TODO: Does this trigger the chat message as well?
		}
	}
}

// == ⚫ COMMAND == //

public record AutoRewardRedeemedCommand(
	AutomaticRewardRedemptionEvent args
	) : IRequest
{
	public TwitchUserDto CreateUserDto()
	{
		return new TwitchUserDto(
			args.RedeemedById,
			args.RedeemedByLogin,
			args.RedeemedById
		);
	}

	public bool RedeemIsPowerUp()
	{
		return args.Reward?.Type switch
		{
			RewardType.SingleMessageBypassSubMode => false,
			RewardType.SendHighlightedMessage => false,
			RewardType.RandomSubEmoteUnlock => false,
			RewardType.ChosenSubEmoteUnlock => false,
			RewardType.ChosenModifiedSubEmoteUnlock => false,
			RewardType.MessageEffect => true,
			RewardType.GigantifyAnEmote => true,
			RewardType.Celebration => true,
			_ => false
		};
	}

	public TwitchCheer CreateCheer(int userId)
	{
		return new TwitchCheer
		{
			Timestamp = DateTimeOffset.UtcNow,
			UserId = userId,
			BitsAmount = args.Reward?.Cost ?? 0,
			Message = args.Message?.Text ?? string.Empty
		};
	}
};

