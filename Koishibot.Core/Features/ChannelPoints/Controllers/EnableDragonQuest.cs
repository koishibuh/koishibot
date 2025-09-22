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
public class EnableDragonQuestController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Dragon Quest"])]
	[HttpPost]
	public async Task<ActionResult> EnableDragonQuest()
	{
		await Mediator.Send(new EnableDragonQuestCommand());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record EnableDragonQuestHandler(
IAppCache Cache,
KoishibotDbContext Database,
IChatReplyService ChatReplyService,
IChannelPointsApi ChannelPointsApi
) : IRequestHandler<EnableDragonQuestCommand>
{
	public async Task Handle(EnableDragonQuestCommand command, CancellationToken cancel)
	{
		var alreadyEnabled = Cache.GetStatusByServiceName(ServiceName.DragonQuest);
		if (alreadyEnabled) throw new CustomException("Dragon Quest already enabled");

		var reward = await Database.GetChannelRewardByName("Dragon Egg Quest");
		if (reward is null) throw new NullException("Reward not found in database");

		var dragonEggQuest = new DragonQuest(reward, 0);
		await Cache
			.AddDragonQuest(dragonEggQuest)
			.UpdateServiceStatusOnline(ServiceName.DragonQuest);

		await ChannelPointsApi.EnableRedemption(reward.TwitchId);
		await ChatReplyService.CreateResponse(Command.DragonQuestEnabled);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record EnableDragonQuestCommand : IRequest;