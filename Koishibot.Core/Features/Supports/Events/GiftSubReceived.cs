using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Subscriptions;
namespace Koishibot.Core.Features.Supports.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsubscriptiongift">Twitch Documentation</see><br/>
/// When a user gives one or more gifted subscriptions in a channel.<br/>
/// Saves to GiftSubscription and SupportTotals tables
/// </summary>
public record GiftSubReceivedHandler(
ISignalrService Signalr,
ITwitchUserHub TwitchUserHub,
KoishibotDbContext Database
) : IRequestHandler<GiftSubReceivedCommand>
{
	public async Task Handle(GiftSubReceivedCommand command, CancellationToken cancel)
	{
		var userDto = command.CreateUserDto();
		var user = await TwitchUserHub.Start(userDto);

		var giftSub = command.CreateGiftSub(user.Id);
		await Database.UpdateEntry(giftSub);

		var supportTotal = await Database.GetSupportTotalByUserId(user.Id);
		if (supportTotal.NotInDatabase())
		{
			supportTotal = command.CreateSupportTotal(user);
		}
		else
		{
			if (command.args.CumulativeTotal is not null)
			{
				supportTotal!.SubsGifted = supportTotal.SubsGifted;
			}
		}

		await Database.UpdateEntry(supportTotal);

		var giftSubVm = command.CreateVm();
		await Signalr.SendStreamEvent(giftSubVm);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record GiftSubReceivedCommand
	(GiftedSubEvent args) : IRequest
{
	public TwitchUserDto CreateUserDto()
	{
		return new TwitchUserDto(
			args.GifterUserId,
			args.GifterUserLogin,
			args.GifterUserName
			);
	}

	public GiftSubscription CreateGiftSub(int userId)
	{
		return new GiftSubscription()
			.Initialize(userId, args.Tier.ToString(), args.Total);
	}

	public SupportTotal CreateSupportTotal(TwitchUser user) =>
		new()
		{
			UserId = user.Id,
			MonthsSubscribed = 0,
			SubsGifted = args.CumulativeTotal ?? 0,
			BitsCheered = 0,
			Tipped = 0
		};

	public StreamEventVm CreateVm() =>
		new()
		{
			EventType = StreamEventType.GiftSub,
			Timestamp = Toolbox.CreateUITimestamp(),
			Message = $"{args.GifterUserName} has gifted {args.Total} {args.Tier} subs!"
		};
}