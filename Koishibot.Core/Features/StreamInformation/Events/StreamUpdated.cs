using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.StreamInformation.Extensions;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs;
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
) : IStreamUpdatedHandler
{
	public async Task Handle(ChannelUpdatedEvent e)
	{
		var category = e.CreateModel();
		await Database.UpsertCategory(category);

		var streamInfo = e.ConvertToDto();
		Cache.UpdateStreamInfo(streamInfo);

		var infoVm = streamInfo.ConvertToVm();
		await Signalr.SendStreamInfo(infoVm);
		await Signalr.SendNewNotification(new NotificationVm("Stream info updated", false));
	}
}

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static class StreamUpdatedEventExtensions
{
	public static StreamInfo ConvertToDto(this ChannelUpdatedEvent e) =>
		new(
			new TwitchUserDto(
				e.BroadcasterId,
				e.BroadcasterLogin,
				e.BroadcasterName),
			e.StreamTitle,
			e.CategoryName,
			e.CategoryId);

	public static StreamCategory CreateModel(this ChannelUpdatedEvent e) => new()
	{
		Name = e.CategoryName,
		TwitchId = e.CategoryId
	};
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IStreamUpdatedHandler
{
	Task Handle(ChannelUpdatedEvent e);
}