using Koishibot.Core.Features.Supports.Extensions;
using Koishibot.Core.Features.Supports.Interfaces;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.TwitchEventSub.Extensions;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
namespace Koishibot.Core.Features.Supports.Events;

// == ⚫ EVENT SUB == //

public class ChannelFollowed(
	IOptions<Settings> Settings,
	EventSubWebsocketClient EventSubClient,
	ITwitchAPI TwitchApi,
	IServiceScopeFactory ScopeFactory
	) : IChannelFollowed
{
	public async Task SetupMethod()
	{
		EventSubClient.ChannelFollow += OnChannelFollowed;
		await SubToEvent();
	}
	public async Task SubToEvent()
	{
		await TwitchApi.CreateEventSubMod
			("channel.follow", "2", Settings);
	}

	private async Task OnChannelFollowed(object sender, ChannelFollowArgs args)
	{
		using var scope = ScopeFactory.CreateScope();
		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

		await mediatr.Send(new ChannelFollowedCommand(args));
	}
}

// == ⚫ COMMAND == //

public record ChannelFollowedCommand
	(ChannelFollowArgs args) : IRequest;


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
		var e = command.args.ConvertToDto();
		var user = await TwitchUserHub.Start(e);

		var follow = new ChannelFollow().Initialize(user);
		await Database.AddFollow(follow);

		var eventVm = follow.ConvertToVm();
		await Signalr.SendStreamEvent(eventVm);
	}
}