using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.StreamElements.Enums;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications.Enums;
using Subscription = Koishibot.Core.Features.Supports.Models.Subscription;
namespace Koishibot.Core.Features.Supports.Events;


/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record ChatNotificationReceivedEventHandler(
ISignalrService Signalr,
ITwitchUserHub TwitchUserHub,
KoishibotDbContext Database
) : IRequestHandler<ChatNotificationReceivedCommand>
{
	public async Task Handle(ChatNotificationReceivedCommand command, CancellationToken cancel)
	{
		var userDto = command.CreateUserDto();
		var user = await TwitchUserHub.Start(userDto);

		if (command.e.NoticeType == NoticeType.CommunitySubGift)
		{
			var supportTotal = await Database.GetSupportTotalByUserId(user.Id);

			if (supportTotal.NotInDatabase())
			{
				supportTotal = command.CreateSupportTotal(user);
			}
			else
			{
				supportTotal!.SubsGifted = command.e.SubGift?.CumulativeTotal ?? 0;
			}

			await Database.UpdateEntry(supportTotal);

			var subVm = command.CreateVm(StreamEventType.CommunityGiftSub);
			await Signalr.SendStreamEvent(subVm);
		}
		else if (command.e.NoticeType == NoticeType.PayItForwardSub)
		{
			var subVm = command.CreateVm(StreamEventType.PayItForward);
			await Signalr.SendStreamEvent(subVm);
		} 
		else
		{
			var sub = command.CreateSub(user.Id);
			await Database.UpdateEntry(sub);
			
			var subVm = command.CreateVm(StreamEventType.Sub);
			await Signalr.SendStreamEvent(subVm);
		}
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record ChatNotificationReceivedCommand(
ChatNotificationEvent e
) : IRequest
{
	public TwitchUserDto CreateUserDto() =>
		new(
			e.ChatterId,
			e.ChatterLogin,
			e.ChatterName);


	public Subscription CreateSub(int userId) =>
		new()
		{
			Timestamp = DateTimeOffset.UtcNow,
			UserId = userId,
			Type = e.NoticeType.ToString(),
			EventMessage =  e.SystemMessage,
			UserMessage = e.Message?.Text
		};

	public SupportTotal CreateSupportTotal(TwitchUser user) =>
		new()
		{
			UserId = user.Id,
			MonthsSubscribed = 1,
			SubsGifted = e.SubGift?.CumulativeTotal ?? 0,
			BitsCheered = 0,
			Tipped = 0
		};	
	

	public StreamEventVm CreateVm(StreamEventType type) =>
		new()
		{
			EventType = type,
			Timestamp = Toolbox.CreateUITimestamp(),
			Message = $"{e.SystemMessage}: {e.Message?.Text}"
		};
}