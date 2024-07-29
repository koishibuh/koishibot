using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Raids.Enums;
using Koishibot.Core.Features.Raids.Extensions;
using Koishibot.Core.Features.RaidSuggestions;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Features.TwitchUsers.Extensions;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Raids;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.Raids.Events;

// == ⚫ COMMAND == //

public record OutgoingRaidCommand(
        RaidEvent args
        ) : IRequest;

// == ⚫ HANDLER == //

public record AddOutgoingRaidHandler(
        IAppCache Cache, ITwitchApiRequest TwitchApiRequest,
		ITwitchIrcService BotIrc,
        KoishibotDbContext Database
        ) : IRequestHandler<OutgoingRaidCommand>
{
    public async Task Handle
        (OutgoingRaidCommand command, CancellationToken cancel)
    {
        var user = new TwitchUserDto(command.args.ToBroadcasterId, command.args.ToBroadcasterLogin, command.args.ToBroadcasterName);

        var raidedUser = await Database.GetUserByTwitchId(user.TwitchId);
        if (raidedUser is null)
        {
            raidedUser = new TwitchUser();
            raidedUser.Initialize(user);
            await Database.UpdateUser(raidedUser);
        }

        var streamId = Cache.GetCurrentStreamId();
        var suggestedById = Cache.GetRaidTargetSuggestorId();

        var raid = new OutgoingRaid();
        raid.Set(streamId, raidedUser.Id, suggestedById);
        await Database.AddRaid(raid);

        await BotIrc.RaidTarget(Code.RaidLeftBehind, user.Name);
    }
}