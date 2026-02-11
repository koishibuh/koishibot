using Koishibot.Core.Features.ChatCommands.Extensions;
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
IServiceScopeFactory ScopeFactory
) : IBitEventReceivedHandler
{
	public async Task Handle(BitsUsedEvent e)
	{
		var userDto = e.CreateUserDto();
		var user = await TwitchUserHub.Start(userDto);

		// TODO: Get stream session Id

		var cheer = e.CreateCheer(user.Id);

		using var scope = ScopeFactory.CreateScope();
		var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();

		database.Cheers.Add(cheer);
		
		var tipJarGoal = await database.GetActiveTipJarGoal();
		if (tipJarGoal is not null)
		{
			tipJarGoal.CurrentAmount = tipJarGoal.CurrentAmount + cheer.BitsAmount;
		}

		await database.SaveChangesAsync();
		//
		// var supportTotal = await database.GetSupportTotalByUserId(user.Id);
		// if (supportTotal.NotInDatabase())
		// {
		// 	supportTotal = command.CreateSupportTotal(user);
		// }
		// else
		// {
		// 	supportTotal!.BitsCheered += cheer.BitsAmount;
		// }
		//
		// await database.UpdateEntry(supportTotal);
		
		var cheerVm = e.CreateCheerVm();
		await Signalr.SendStreamEvent(cheerVm);
	}
}

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static class BitsUsedEventExtensions
{
	public static TwitchUserDto CreateUserDto(this BitsUsedEvent e) =>
		new(
			e.BroadcasterId,
			e.BroadcasterLogin,
			e.BroadcasterName);

	public static StreamEventVm CreateCheerVm(this BitsUsedEvent e)
	{
		var message = e.PowerUpData is not null
			? $"{e.CheererName} has cheered {e.BitAmount} with {e.PowerUpData.Type}"
			: $"{e.CheererName} has cheered {e.BitAmount}";

		return new StreamEventVm()
		{
			EventType = StreamEventType.Cheer,
			Timestamp = (DateTimeOffset.UtcNow).ToString("yyyy-MM-dd HH:mm"),
			Message = message,
			Amount = e.BitAmount,
		};
	}
	
	public static TwitchCheer CreateCheer(this BitsUsedEvent e, int userId) => new()
	{
		Timestamp = DateTimeOffset.UtcNow,
		UserId = userId,
		BitsAmount = e.BitAmount,
		Message = e.Message?.Text ?? ""
		// StreamSessionId = null
	};

	public static SupportTotal CreateSupportTotal(this BitsUsedEvent e, TwitchUser user) => new()
	{
		UserId = user.Id,
		MonthsSubscribed = 0,
		SubsGifted = 0,
		BitsCheered = e.BitAmount,
		Tipped = 0
	};
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IBitEventReceivedHandler
{
	Task Handle(BitsUsedEvent e);
}