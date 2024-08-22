using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.StreamInformation.Extensions;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelUpdate;
namespace Koishibot.Core.Features.StreamInformation.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelupdate">Channel Update EventSub Documentation</see></para>
/// <para>Raised when there are changes in title, category, mature flag, broadcast, or language</para>
/// <para>Event sent to <see cref="StreamInfoUpdatedEventHandler"/></para>
/// </summary>
public record StreamUpdatedHandler(
IAppCache Cache,
ISignalrService Signalr,
KoishibotDbContext Database
) : IRequestHandler<StreamUpdatedCommand>
{
	public async Task Handle
		(StreamUpdatedCommand command, CancellationToken cancellationToken)
	{
		var category = command.CreateModel();
		await category.UpsertEntry(Database);

		var streamInfo = command.ConvertToDto();

		Cache.UpdateStreamInfo(streamInfo);

		var infoVm = streamInfo.ConvertToVm();
		await Signalr.SendStreamInfo(infoVm);
		await Signalr.SendNewNotification(new NotificationVm("Stream info updated", false));
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record StreamUpdatedCommand(ChannelUpdatedEvent e) : IRequest
{
	public StreamInfo ConvertToDto()
	{
		return new StreamInfo(
		new TwitchUserDto(
		e.BroadcasterId,
		e.BroadcasterLogin,
		e.BroadcasterName),
		e.StreamTitle,
		e.CategoryName,
		e.CategoryId);
	}

	public StreamCategory CreateModel()
	{
		return new StreamCategory
		{
		Name = e.CategoryName,
		TwitchId = e.CategoryId
		};
	}
}