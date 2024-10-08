using Koishibot.Core.Features.ChannelPoints.Extensions;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;
namespace Koishibot.Core.Features.Supports.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
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
			var channelPointReward = await Database.GetChannelRewardByName(command.args.Reward.Type.ToString());
			if (channelPointReward.NotInDatabase())
			{
				// TODO: Twitch doesn't show the correct value for cost, need to get this from DB
				channelPointReward = new ChannelPointReward
				{
					CreatedOn = DateTimeOffset.UtcNow,
					TwitchId = command.args.Reward.Type.ToString(),
					Title = command.args.Reward.Type.ToString(),
					Cost = 30,
					IsEnabled = true,
					IsUserInputRequired = false,
					IsMaxPerStreamEnabled = false,
					IsMaxPerUserPerStreamEnabled = false,
					IsGlobalCooldownEnabled = false,
					IsPaused = false,
					ShouldRedemptionsSkipRequestQueue = true
				};

				await Database.UpdateEntry(channelPointReward);

			}

			var cheer = command.CreateCheer(user.Id, channelPointReward!.Cost);
			await Database.UpdateEntry(cheer);


			var supportTotal = await Database.GetSupportTotalByUserId(user.Id);
			if (supportTotal.NotInDatabase())
			{
				supportTotal = command.CreateSupportTotal(user);
			}
			else
			{
				supportTotal!.BitsCheered += cheer.BitsAmount;
			}

			await Database.UpdateEntry(supportTotal);

			var vm = cheer.CreateVm(user.Name, command.args.Reward.Type);
			await Signalr.SendStreamEvent(vm);

			// TODO: Do thing to chat message? Like with large emotes
		}
		else
		{
			var channelPointReward = await Database.GetChannelRewardByName(command.args.Reward.Type.ToString());
			if (channelPointReward.NotInDatabase())
			{
				channelPointReward = new ChannelPointReward
				{
					CreatedOn = DateTimeOffset.UtcNow,
					TwitchId = command.args.Reward.Type.ToString(),
					Title = command.args.Reward.Type.ToString(),
					Cost = command.args.Reward.Cost,
					IsEnabled = true,
					IsUserInputRequired = false,
					IsMaxPerStreamEnabled = false,
					IsMaxPerUserPerStreamEnabled = false,
					IsGlobalCooldownEnabled = false,
					IsPaused = false,
					ShouldRedemptionsSkipRequestQueue = true
				};

				await Database.UpdateEntry(channelPointReward);
			}

			var redemption = command.CreateRedemption(channelPointReward.Id, user);
			await Database.UpdateEntry(redemption);

			var vm = command.CreateVm();
			await Signalr.SendStreamEvent(vm);
		}
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record AutoRewardRedeemedCommand(
AutomaticRewardRedemptionEvent args
) : IRequest
{
	public TwitchUserDto CreateUserDto()
	{
		return new TwitchUserDto(
			args.RedeemedById,
			args.RedeemedByLogin,
			args.RedeemedByName
			);
	}


	public RedeemedRewardDto ConvertToDto()
		=> new(
			new TwitchUserDto(args.RedeemedById, args.RedeemedByLogin, args.RedeemedById),
			args.Message.Text,
			args.RedeemedAt,
			args.Reward?.Type.ToString(),
			"0",
			args.Reward.Type.ToString(),
			true.ToString(),
			args.Reward?.Cost);

	public ChannelPointRedemption CreateRedemption(int id, TwitchUser user) =>
		new()
		{
			ChannelPointRewardId = id,
			Timestamp = args.RedeemedAt,
			UserId = user.Id,
			WasSuccesful = true
		};

	public bool RedeemIsPowerUp() =>
		args.Reward?.Type switch
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

	public TwitchCheer CreateCheer(int userId, int cost) =>
		new()
		{
			Timestamp = DateTimeOffset.UtcNow,
			UserId = userId,
			BitsAmount = cost,
			Message = args.Message?.Text ?? string.Empty
		};

	public SupportTotal CreateSupportTotal(TwitchUser user) =>
		new()
		{
			UserId = user.Id,
			MonthsSubscribed = 0,
			SubsGifted = 0,
			BitsCheered = args.Reward.Cost,
			Tipped = 0
		};

	public StreamEventVm CreateVm()
	{
		return new StreamEventVm
		{
			EventType = StreamEventType.ChannelPoint,
			Timestamp = Toolbox.CreateUITimestamp(),
			Message = $"{args.RedeemedByName} has redeemed {args.Reward.Type.ToString()}"
		};
	}
}