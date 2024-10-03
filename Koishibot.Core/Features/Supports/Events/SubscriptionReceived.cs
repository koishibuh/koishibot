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
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record SubscriptionReceivedHandler(
ISignalrService Signalr,
ITwitchUserHub TwitchUserHub,
KoishibotDbContext Database
) : IRequestHandler<SubscriptionReceivedCommand>
{
	public async Task Handle(SubscriptionReceivedCommand command, CancellationToken cancel)
	{
		var userDto = command.CreateUserDto();
		var user = await TwitchUserHub.Start(userDto);

		var sub = command.CreateSub(user.Id);
		await Database.UpdateEntry(sub);

		var supportTotal = await Database.GetSupportTotalByUserId(user.Id);

		// This does not have month info
		if (supportTotal.NotInDatabase())
		{
			supportTotal = command.CreateSupportTotal(user);
		}
		else
		{
			supportTotal!.MonthsSubscribed = supportTotal.MonthsSubscribed == 0
				? 1
				: supportTotal.MonthsSubscribed += 1;
		}

		await Database.UpdateEntry(supportTotal);

		var subVm = command.CreateVm();
		await Signalr.SendStreamEvent(subVm);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record SubscriptionReceivedCommand(
SubscriptionEvent e
) : IRequest
{
	public TwitchUserDto CreateUserDto() =>
		new(
			e.SubscriberId,
			e.SubscriberLogin,
			e.SubscriberName);


	public Subscription CreateSub(int userId) =>
		new()
		{
			Timestamp = DateTimeOffset.UtcNow,
			UserId = userId,
			Tier = e.Tier.ToString(),
			Message = string.Empty,
			Gifted = e.IsGift
		};

	public SupportTotal CreateSupportTotal(TwitchUser user) =>
		new()
		{
			UserId = user.Id,
			MonthsSubscribed = 1,
			SubsGifted = 0,
			BitsCheered = 0,
			Tipped = 0
		};

	public StreamEventVm CreateVm() =>
		new()
		{
			EventType = StreamEventType.Sub,
			Timestamp = Toolbox.CreateUITimestamp(),
			Message = $"{e.SubscriberName} has subscribed at {e.Tier} for 1 month"
		};
}