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
public record ResubReceivedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<ResubReceivedCommand>
{
	public async Task Handle(ResubReceivedCommand command, CancellationToken cancel)
	{
		var userDto = command.CreateUserDto();
		var user = await TwitchUserHub.Start(userDto);

		var sub = command.CreateSub(user.Id);
		await Database.UpdateEntry(sub);

		await Database.UpdateSubDurationTotal(sub, command.e.CumulativeMonths);

		var resubVm = command.CreateVm();
		await Signalr.SendStreamEvent(resubVm);
	}
}

// == ⚫ COMMAND == //

public record ResubReceivedCommand(
	ResubMessageEvent e
	) : IRequest
{
	public TwitchUserDto CreateUserDto()
	{
		return new TwitchUserDto(
			e.ResubscriberId,
			e.ResubscriberLogin,
			e.ResubscriberName);
	}

	public Subscription CreateSub(int userId)
	{
		return new Subscription
		{
			Timestamp = DateTimeOffset.UtcNow,
			UserId = userId,
			Tier = e.Tier.ToString(),
			Message = e.Message?.Text ?? string.Empty,
			Gifted = false
		};
	}

	public StreamEventVm CreateVm()
	{
		return new StreamEventVm
		{
			EventType = StreamEventType.Sub,
			Timestamp = (DateTimeOffset.UtcNow).ToString("yyyy-MM-dd HH:mm"),
			Message = $"{e.ResubscriberName} has resubscribed at {e.Tier} for {e.DurationMonths} months"
		};
	}
};