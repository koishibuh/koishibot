using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Cheers;
namespace Koishibot.Core.Features.Supports.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelcheer">Channel Cheer EventSub Documentation</see></para>
/// </summary>
public record CheerReceivedHandler(
ISignalrService Signalr,
ITwitchUserHub TwitchUserHub,
KoishibotDbContext Database
) : IRequestHandler<CheerReceivedCommand>
{
	public async Task Handle
		(CheerReceivedCommand command, CancellationToken cancel)
	{
		var userDto = command.CreateUserDto();
		var user = await TwitchUserHub.Start(userDto);

		var cheer = command.CreateCheer(user.Id);
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

		var cheerVm = command.CreateVm();
		await Signalr.SendStreamEvent(cheerVm);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record CheerReceivedCommand(
CheerReceivedEvent args
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
			Message = args.Message
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
};