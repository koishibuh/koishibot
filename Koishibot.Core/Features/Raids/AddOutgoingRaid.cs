//using Koishibot.Core.Features.Common;
//using Koishibot.Core.Features.Raids.Enums;
//using Koishibot.Core.Features.Raids.Extensions;
//using Koishibot.Core.Features.RaidSuggestions;
//using Koishibot.Core.Features.RaidSuggestions.Models;
//using Koishibot.Core.Features.TwitchUsers.Extensions;
//using Koishibot.Core.Features.TwitchUsers.Models;
//using Koishibot.Core.Persistence;
//using Koishibot.Core.Services.TwitchEventSub.Extensions;
//using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
//namespace Koishibot.Core.Features.Raids;

//// == ⚫ COMMAND == //

//public record OutgoingRaidCommand(
//		ChannelRaidArgs args
//		) : IRequest;

//// == ⚫ HANDLER == //

//public record AddOutgoingRaidHandler(
//		IAppCache Cache, IChatMessageService BotIrc,
//		KoishibotDbContext Database
//		) : IRequestHandler<OutgoingRaidCommand>
//{
//	public async Task Handle
//		(OutgoingRaidCommand command, CancellationToken cancel)
//	{
//		var (user, _) = command.args.ConvertToDto();

//		var raidedUser = await Database.GetUserByTwitchId(user.TwitchId);
//		if (raidedUser is null)
//		{
//			raidedUser = new TwitchUser();
//			raidedUser.Initialize(user);
//			await Database.UpdateUser(raidedUser);
//		}

//		var streamId = Cache.GetCurrentStreamId();
//		var suggestedById = Cache.GetRaidTargetSuggestorId();

//		var raid = new OutgoingRaid();
//		raid.Set(streamId, raidedUser.Id, suggestedById);
//		await Database.AddRaid(raid);

//		await BotIrc.RaidTarget(Code.RaidLeftBehind, user.Name);
//	}
//}