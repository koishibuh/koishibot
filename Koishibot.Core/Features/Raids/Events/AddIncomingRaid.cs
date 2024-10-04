using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Raids.Interfaces;
using Koishibot.Core.Features.Raids.Models;
using Koishibot.Core.Features.RaidSuggestions.Enums;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Features.StreamInformation.ViewModels;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Raids;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.Raids.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
public record AddIncomingRaidHandler(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest,
ITwitchUserHub TwitchUserHub,
ISignalrService Signalr,
KoishibotDbContext Database,
IPromoVideoService PromoVideoService,
IChatReplyService ChatReplyService
) : IRequestHandler<IncomingRaidCommand>
{
	public async Task Handle(IncomingRaidCommand command, CancellationToken cancellationToken)
	{
		var userDto = command.CreateUserDto();
		var user = await TwitchUserHub.Start(userDto);

		await SendShoutout(user.TwitchId);

		var videoUrl = await PromoVideoService.Start(user);
		if (videoUrl is not null)
		{
			await Signalr.SendPromoVideoUrl(videoUrl);
		}

		// TODO: Figure out setup in OBS
		// Check if OBS has a way to open interact window for video
		// Overlay have follow clicky animation after shoutout

		// Get Streamer Info for chat message

		var streamInfo = await GetStreamInfo(user.TwitchId);

		// Add category to database
		await Database.GetStreamCategoryId(streamInfo.Category, streamInfo.CategoryId);

		await ChatReplyService.App(Command.RaidReceived);

		var incomingRaid = new IncomingRaid
		{
			Timestamp = DateTimeOffset.UtcNow,
			RaidedByUserId = user.Id,
			ViewerCount = command.args.ViewerCount
		};

		await Database.UpdateEntry(incomingRaid);

		var vm = incomingRaid.CreateVm();
		await Signalr.SendStreamEvent(vm);
	}

	/*══════════════════【】═════════════════*/
	private async Task SendShoutout(string streamerId)
	{
		var parameters = new SendShoutoutParameters
		{
			FromBroadcasterId = Settings.Value.StreamerTokens.UserId,
			ToBroadcasterId = streamerId,
			ModeratorId = Settings.Value.StreamerTokens.UserId
		};

		await TwitchApiRequest.SendShoutout(parameters);
	}

	private async Task<StreamInfoVm?> GetStreamInfo(string userId)
	{
		var parameters = new GetChannelInfoQueryParameters
		{
			BroadcasterIds = [userId]
		};

		var response = await TwitchApiRequest.GetChannelInfo(parameters);
		if (response.IsEmpty())
		{
			// do a thing
			return null;
		}
		var stream = response[0];
		return new StreamInfoVm(stream.StreamTitle, stream.CategoryName, stream.CategoryId);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record IncomingRaidCommand(RaidEvent args) : IRequest
{
	public TwitchUserDto CreateUserDto() => new(
		args.FromBroadcasterId,
		args.FromBroadcasterLogin,
		args.FromBroadcasterName);
}