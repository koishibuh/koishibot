using Koishibot.Core.Features.Supports.Extensions;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Follow;
namespace Koishibot.Core.Features.Supports.Events;

// == ⚫ COMMAND == //

public record ChannelFollowedCommand(FollowEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelfollow">Channel Followed EventSub Documentation</see></para>
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public record ChannelFollowedHandler(
	IAppCache Cache, ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<ChannelFollowedCommand>
{
	public async Task Handle
		(ChannelFollowedCommand command, CancellationToken cancellationToken)
	{
		var userDto = new TwitchUserDto
			(command.args.FollowerId, command.args.FollowerLogin, command.args.FollowerName);

		var user = await TwitchUserHub.Start(userDto);

		var follow = new ChannelFollow().Initialize(user);
		await Database.AddFollow(follow);

		var eventVm = follow.ConvertToVm();
		await Signalr.SendStreamEvent(eventVm);
	}
}