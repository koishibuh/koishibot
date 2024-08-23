using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.ChannelPoints.Enums;
using Koishibot.Core.Features.ChannelPoints.Extensions;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.ChannelPoints.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/dragon-quest")]
public class DisableDragonQuestController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Dragon Quest"])]
	[HttpDelete]
	public async Task<ActionResult> DisableDragonQuest()
	{
		await Mediator.Send(new DisableDragonQuestCommand());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record DisableDragonQuestHandler(
IAppCache Cache,
KoishibotDbContext Database,
IChatReplyService ChatReplyService,
IChannelPointsApi ChannelPointsApi
) : IRequestHandler<EnableDragonQuestCommand>
{
	public async Task Handle(EnableDragonQuestCommand command, CancellationToken cancel)
	{
		var alreadyDisabled = Cache.GetStatusByServiceName(ServiceName.DragonEggQuest);
		if (alreadyDisabled) throw new CustomException("Dragon Quest already disabled");

		var reward = await Database.GetChannelRewardByName("Dragon Egg Quest");
		if (reward is null) throw new NullException("Reward not found in database");

		var dragonEggQuest = new DragonEggQuest().Set(reward, 0);
		await Cache
		.AddDragonEggQuest(dragonEggQuest)
		.UpdateServiceStatusOffline(ServiceName.DragonEggQuest);

		await ChannelPointsApi.DisableRedemption(reward.TwitchId);
		await ChatReplyService.App(Command.DragonEggQuestDisabled);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record DisableDragonQuestCommand : IRequest;