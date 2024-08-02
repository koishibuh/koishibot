using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Extensions;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Follow;
namespace Koishibot.Core.Features.Supports.Events;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelfollow">Channel Followed EventSub Documentation</see></para>
/// </summary>
public record ChannelFollowedHandler(
	IAppCache Cache, ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<ChannelFollowedCommand>
{
	public async Task Handle
		(ChannelFollowedCommand command, CancellationToken cancellationToken)
	{
		var userDto = command.CreateUserDto();

		var user = await TwitchUserHub.Start(userDto);

		var follow = new ChannelFollow().Initialize(user);
		await Database.AddFollow(follow);

		var eventVm = command.CreateVm();
		await Signalr.SendStreamEvent(eventVm);
	}
}

// == ⚫ COMMAND == //

public record ChannelFollowedCommand
	(FollowEvent args) : IRequest
{
	public TwitchUserDto CreateUserDto()
	{
		return new TwitchUserDto(
			args.FollowerId,
			args.FollowerLogin,
			args.FollowerName);
	}

	public StreamEventVm CreateVm()
	{
		return new StreamEventVm
		{
			EventType = StreamEventType.Follow,
			Timestamp = (DateTimeOffset.UtcNow).ToString("yyyy-MM-dd HH:mm"),
			Message = $"{args.FollowerName} has followed"
		};
	}
};