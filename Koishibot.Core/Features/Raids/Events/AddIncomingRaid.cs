using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Raids.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Raids;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.Raids.Events;

// == ⚫ COMMAND == //

public record IncomingRaidCommand(
				RaidEvent args
				) : IRequest;

// == ⚫ HANDLER == //

public record AddIncomingRaidHandler(
		IOptions<Settings> Settings,
		ITwitchApiRequest TwitchApiRequest,
		ITwitchIrcService BotIrc,
		ITwitchUserHub TwitchUserHub,
		ISignalrService Signalr,
		//IShoutoutApi ShoutoutApi,
		IAppCache Cache,
		KoishibotDbContext Database,
		 IPromoVideoService PromoVideoService
		) : IRequestHandler<IncomingRaidCommand>
{
	public async Task Handle(IncomingRaidCommand command, CancellationToken cancellationToken)
	{
		var e = new TwitchUserDto(command.args.FromBroadcasterId, command.args.FromBroadcasterLogin, command.args.FromBroadcasterName);

		// do obs things 

		// send link to client through signalR

		// TODO: handle linkydo on the client

		var user = await TwitchUserHub.Start(e);

		//var stream = await ShoutoutApi.GetStreamInfo(user.TwitchId);

		//var videoUrl = await PromoVideoService.Start(user);
		//if (videoUrl is not null)
		//{
		//	await Signalr.SendPromoVideoUrl(videoUrl);
		//}

		//await ShoutoutApi.SendShoutout(user.TwitchId);
		//await BotIrc.RaidedBy(stream);

		//var streamId = Cache.GetCurrentStreamId();

		//IncomingRaid raid = new();
		//raid.Set(streamId, user.Id, command.args.ViewerCount);
		//await Database.AddRaid(raid);

		//var raidVm = raid.ConvertToModel();
		//Cache.AddStreamEvent(raidVm);
		//await Signalr.SendStreamEvent(raidVm);
	}
}

// == ⚫ CHAT REPLY  == //

public static class IncomingRaidChatReply
{
	public static async Task RaidedBy(this ITwitchIrcService botIrc, StreamInfo e)
	{
		await botIrc.BotSend($"We've been raided by {e.Streamer.Name} who was streaming {e.StreamTitle} ({e.Category})");
	}
}
