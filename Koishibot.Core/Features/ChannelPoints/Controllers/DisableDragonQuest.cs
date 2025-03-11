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
) : IRequestHandler<DisableDragonQuestCommand>
{
	public async Task Handle(DisableDragonQuestCommand command, CancellationToken cancel)
	{
		var enabled = Cache.GetStatusByServiceName(ServiceName.DragonQuest);
		if (enabled is false) throw new CustomException("Dragon Quest already disabled");

		var reward = await Database.GetChannelRewardByName("Dragon Egg Quest");
		if (reward is null) throw new NullException("Reward not found in database");
		await ChannelPointsApi.DisableRedemption(reward.TwitchId);

		await Cache
			.RemoveDragonQuest()
			.UpdateServiceStatusOffline(ServiceName.DragonQuest);

		await ChatReplyService.App(Command.DragonQuestClosed);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record DisableDragonQuestCommand : IRequest;