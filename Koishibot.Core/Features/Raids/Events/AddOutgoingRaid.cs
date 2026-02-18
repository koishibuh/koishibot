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
IAppCache Cache,
IChatReplyService ChatReplyService,
KoishibotDbContext Database
) : IOutgoingRaidHandler
{
	public async Task Handle(RaidEvent e)
	{
		var user = e.CreateUserDto();

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

		await ChatReplyService.CreateResponse(Response.RaidLeftBehind);
	}
}

/*═══════════════════【 INTERFACE 】═══════════════════*/

public interface IOutgoingRaidHandler
{
	public Task Handle(RaidEvent e);
}