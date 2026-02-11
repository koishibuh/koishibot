using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
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
) : IChatNotificationReceivedEventHandler
{
	public async Task Handle(ChatNotificationEvent command)
	{
		var userDto = command.CreateUserDto();
		var user = await TwitchUserHub.Start(userDto);

		if (command.NoticeType == NoticeType.CommunitySubGift)
		{
			var supportTotal = await Database.GetSupportTotalByUserId(user.Id);

			if (supportTotal.NotInDatabase())
			{
				supportTotal = command.CreateSupportTotal(user);
			}
			else
			{
				supportTotal!.SubsGifted = command.SubGift?.CumulativeTotal ?? 0;
			}

			await Database.UpdateEntry(supportTotal);

			var subVm = command.CreateVm(StreamEventType.CommunityGiftSub);
			await Signalr.SendStreamEvent(subVm);
		}
		else if (command.NoticeType == NoticeType.PayItForwardSub)
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

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static class ChatNotificationEventExtensions
{
	public static TwitchUserDto CreateUserDto(this ChatNotificationEvent e) =>
		new(
			e.ChatterId,
			e.ChatterLogin,
			e.ChatterName);
	
	public static Subscription CreateSub(this ChatNotificationEvent e, int userId) =>
		new()
		{
			Timestamp = DateTimeOffset.UtcNow,
			UserId = userId,
			Type = e.NoticeType.ToString(),
			EventMessage =  e.SystemMessage,
			UserMessage = e.Message?.Text
		};

	public static SupportTotal CreateSupportTotal(this ChatNotificationEvent e, TwitchUser user) =>
		new()
		{
			UserId = user.Id,
			MonthsSubscribed = 1,
			SubsGifted = e.SubGift?.CumulativeTotal ?? 0,
			BitsCheered = 0,
			Tipped = 0
		};	
	
	public static StreamEventVm CreateVm(this ChatNotificationEvent e, StreamEventType type) =>
		new()
		{
			EventType = type,
			Timestamp = Toolbox.CreateUITimestamp(),
			Message = $"{e.SystemMessage}: {e.Message?.Text}"
		};
}


/*══════════════════【 INTERFACE 】══════════════════*/
public interface IChatNotificationReceivedEventHandler
{
	Task Handle(ChatNotificationEvent e);
}