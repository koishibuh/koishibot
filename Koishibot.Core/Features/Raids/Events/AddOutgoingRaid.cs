using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Raids.Extensions;
using Koishibot.Core.Features.RaidSuggestions.Enums;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Raids;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.Raids.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
public record AddOutgoingRaidHandler(
IAppCache Cache, ITwitchApiRequest TwitchApiRequest,
IChatReplyService ChatReplyService,
KoishibotDbContext Database
) : IRequestHandler<OutgoingRaidCommand>
{
	public async Task Handle
		(OutgoingRaidCommand command, CancellationToken cancel)
	{
		var user = command.CreateDto();

		var raidedUser = await Database.GetUserByTwitchId(user.TwitchId);
		if (raidedUser is null)
		{
			raidedUser = new TwitchUser().Initialize(user);
			await Database.UpdateUser(raidedUser);
		}

		var streamId = Cache.GetCurrentStreamId();
		var suggestedById = Cache.GetRaidTargetSuggestorId();

		var raid = new OutgoingRaid().Set(streamId, raidedUser.Id, suggestedById);
		await Database.AddRaid(raid);

		await ChatReplyService.App(Command.RaidLeftBehind);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record OutgoingRaidCommand(RaidEvent args) : IRequest
{
	public TwitchUserDto CreateDto()
		=> new(args.ToBroadcasterId, args.ToBroadcasterLogin, args.ToBroadcasterName);
}