using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Extensions;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Subscriptions;

namespace Koishibot.Core.Features.Supports.Events;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record SubscriptionReceivedHandler(
	IAppCache Cache,
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

		// This does not have month info
		await Database.UpdateSubDurationTotal(sub, 1);

		var subVm = command.CreateVm();
		await Signalr.SendStreamEvent(subVm);
	}
}

// == ⚫ COMMAND == //

public record SubscriptionReceivedCommand(
	SubscriptionEvent e
	) : IRequest
{
	public TwitchUserDto CreateUserDto()
	{
		return new TwitchUserDto(
			e.SubscriberId,
			e.SubscriberLogin,
			e.SubscriberName);
	}

	public Subscription CreateSub(int userId)
	{
		return new Subscription
		{
			Timestamp = DateTimeOffset.UtcNow,
			UserId = userId,
			Tier = e.Tier.ToString(),
			Message = string.Empty,
			Gifted = e.IsGift
		};
	}

	public StreamEventVm CreateVm()
	{
		return new StreamEventVm
		{
			EventType = StreamEventType.Sub,
			Timestamp = (DateTimeOffset.UtcNow).ToString("yyyy-MM-dd HH:mm"),
			Message = $"{e.SubscriberName} has subscribed at {e.Tier} for 1 month"
		};
	}
};