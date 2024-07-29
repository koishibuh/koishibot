using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.StreamInformation.Extensions;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelUpdate;

namespace Koishibot.Core.Features.StreamInformation.Events;

// == ⚫ COMMAND == //

public record StreamUpdatedCommand(ChannelUpdatedEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelupdate">Channel Update EventSub Documentation</see></para>
/// <para>Raised when there are changes in title, category, mature flag, broadcast, or language</para>
/// <para>Event sent to <see cref="StreamInfoUpdatedEventHandler"/></para>
/// </summary>
public record StreamUpdatedHandler(
	IAppCache Cache, ISignalrService Signalr,
	ILogger<StreamUpdatedHandler> Log
	) : IRequestHandler<StreamUpdatedCommand>
{
	public async Task Handle
		(StreamUpdatedCommand command, CancellationToken cancellationToken)
	{
		var e = new StreamInfo(
			new TwitchUserDto(
				command.args.BroadcasterId,
				command.args.BroadcasterLogin,
				command.args.BroadcasterName),
				command.args.StreamTitle,
				command.args.CategoryName,
				command.args.CategoryId);

		Cache.UpdateStreamInfo(e);

		var infoVm = e.ConvertToVm();
		await Signalr.SendStreamInfo(infoVm);
	}
}