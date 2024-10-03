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
public record ResubReceivedHandler(
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

		var supportTotal = await Database.GetSupportTotalByUserId(user.Id);
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

		var resubVm = command.CreateVm();
		await Signalr.SendStreamEvent(resubVm);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record ResubReceivedCommand(
ResubMessageEvent e
) : IRequest
{
	public TwitchUserDto CreateUserDto() =>
		new(
			e.ResubscriberId,
			e.ResubscriberLogin,
			e.ResubscriberName);

	public Subscription CreateSub(int userId) =>
		new()
		{
			Timestamp = DateTimeOffset.UtcNow,
			UserId = userId,
			Tier = e.Tier.ToString(),
			Message = e.Message?.Text ?? string.Empty,
			Gifted = false
		};

	public SupportTotal CreateSupportTotal(TwitchUser user) =>
		new()
		{
			UserId = user.Id,
			MonthsSubscribed = e.CumulativeMonths,
			SubsGifted = 0,
			BitsCheered = 0,
			Tipped = 0
		};

	public StreamEventVm CreateVm() => new()
	{
		EventType = StreamEventType.Sub,
		Timestamp = Toolbox.CreateUITimestamp(),
		Message = $"{e.ResubscriberName} has resubscribed at {e.Tier} for {e.CumulativeMonths} months"
	};
}