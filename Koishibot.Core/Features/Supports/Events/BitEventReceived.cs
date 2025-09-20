using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Cheers;
namespace Koishibot.Core.Features.Supports.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelbitsuse">EventSub Documentation</see></para>
/// </summary>
public record BitEventReceivedHandler(
ISignalrService Signalr,
ITwitchUserHub TwitchUserHub,
IServiceScopeFactory ScopeFactory,
KoishibotDbContext Database
) : IRequestHandler<BitEventReceivedCommand>
{
	public async Task Handle
		(BitEventReceivedCommand command, CancellationToken cancel)
	{
		var userDto = command.CreateUserDto();
		var user = await TwitchUserHub.Start(userDto);

		// TODO: Get stream session Id
		
		var cheer = command.CreateCheer(user.Id);
		
		using var scope = ScopeFactory.CreateScope();
		var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();
		
		await database.AddEntry(cheer);

		var supportTotal = await database.GetSupportTotalByUserId(user.Id);
		if (supportTotal.NotInDatabase())
		{
			supportTotal = command.CreateSupportTotal(user);
		}
		else
		{
			supportTotal!.BitsCheered += cheer.BitsAmount;
		}

		await database.UpdateEntry(supportTotal);

		var cheerVm = command.CreateVm();
		await Signalr.SendStreamEvent(cheerVm);
		
		var cheerGoalVm = command.CreateGoalEventVm();
		await Signalr.SendGoalEvent(cheerGoalVm);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record BitEventReceivedCommand(
BitsUsedEvent args
) : IRequest
{
	public TwitchUserDto CreateUserDto() =>
		new(
			args.CheererId,
			args.CheererLogin,
			args.CheererName);

	public TwitchCheer CreateCheer(int userId) =>
		new()
		{
			Timestamp = DateTimeOffset.UtcNow,
			UserId = userId,
			BitsAmount = args.BitAmount,
			Message = args.Message?.Text ?? ""
			// StreamSessionId = null
		};

	public SupportTotal CreateSupportTotal(TwitchUser user) =>
		new()
		{
			UserId = user.Id,
			MonthsSubscribed = 0,
			SubsGifted = 0,
			BitsCheered = args.BitAmount,
			Tipped = 0
		};

	public StreamEventVm CreateVm() =>
		new()
		{
			EventType = StreamEventType.Cheer,
			Timestamp = (DateTimeOffset.UtcNow).ToString("yyyy-MM-dd HH:mm"),
			Message = $"{args.CheererName} has cheered {args.BitAmount}"
		};

	public GoalEventVm CreateGoalEventVm() =>
		new("Bits", args.BitAmount);
};

public record GoalEventVm(string GoalType, int Amount);