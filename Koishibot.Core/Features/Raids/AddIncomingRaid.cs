using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Raids.Extensions;
using Koishibot.Core.Features.Raids.Interfaces;
using Koishibot.Core.Features.Raids.Models;
using Koishibot.Core.Features.Shoutouts;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.TwitchEventSub.Extensions;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
namespace Koishibot.Core.Features.Raids;

// == ⚫ COMMAND == //

public record IncomingRaidCommand(
		ChannelRaidArgs args
		) : IRequest;

// == ⚫ HANDLER == //

public record AddIncomingRaidHandler(
	ITwitchUserHub TwitchUserHub,
	ISignalrService Signalr,
	IShoutoutApi ShoutoutApi, IAppCache Cache, 
	KoishibotDbContext Database,
	IChatMessageService BotIrc, IPromoVideoService PromoVideoService
	) : IRequestHandler<IncomingRaidCommand>
{
	public async Task Handle(IncomingRaidCommand command, CancellationToken cancellationToken)
	{
		var e = command.args.ConvertToDto();

		// do obs things 

		// send link to client through signalR

		// TODO: handle linkydo on the client

		var user = await TwitchUserHub.Start(e.user);

		var stream = await ShoutoutApi.GetStreamInfo(e.user.TwitchId);

		var videoUrl = await PromoVideoService.Start(user);
		if (videoUrl is not null)
		{
			await Signalr.SendPromoVideoUrl(videoUrl);
		}

		await ShoutoutApi.SendShoutout(user.TwitchId);
		await BotIrc.RaidedBy(stream);

		var streamId = Cache.GetCurrentStreamId();

		IncomingRaid raid = new();
		raid.Set(streamId, user.Id, e.viewerCount);
		await Database.AddRaid(raid);

		var raidVm = raid.ConvertToModel();
		Cache.AddStreamEvent(raidVm);
		await Signalr.SendStreamEvent(raidVm);
	}
}

// == ⚫ CHAT REPLY  == //

public static class IncomingRaidChatReply
{
	public static async Task RaidedBy(this IChatMessageService botIrc, StreamInfo e)
	{
		await botIrc.BotSend($"We've been raided by {e.Streamer.Name} who was streaming {e.StreamTitle} ({e.Category})");
	}
}
