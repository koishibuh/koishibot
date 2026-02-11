using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Follow;
namespace Koishibot.Core.Features.Supports.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelfollow">Channel Followed EventSub Documentation</see></para>
/// </summary>
public record ChannelFollowedHandler(
ISignalrService Signalr,
ITwitchUserHub TwitchUserHub,
KoishibotDbContext Database
) : IChannelFollowedHandler
{
	public async Task Handle(FollowEvent e)
	{
		var userDto = e.CreateUserDto();
		var user = await TwitchUserHub.Start(userDto, true);

		var follow = new ChannelFollow(user.Id);
		await Database.UpdateEntry(follow);

		var eventVm = e.CreateVm();
		await Signalr.SendStreamEvent(eventVm);
	}
}

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static class StreamUpdatedEventExtensions
{
	public static TwitchUserDto CreateUserDto(this FollowEvent e) =>
		new(
			e.FollowerId,
			e.FollowerLogin,
			e.FollowerName);

	public static StreamEventVm CreateVm(this FollowEvent e) => new()
	{
		EventType = StreamEventType.Follow,
		Timestamp = Toolbox.CreateUITimestamp(),
		Message = $"{e.FollowerName} has followed"
	};
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IChannelFollowedHandler
{
	Task Handle(FollowEvent e);
}