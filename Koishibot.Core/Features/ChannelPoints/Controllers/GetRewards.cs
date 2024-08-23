using Koishibot.Core.Features.ChannelPoints.Extensions;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.ChannelPoints.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/point-rewards")]
public class GetChannelPointRewardsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Point Reward"])]
	[HttpGet]
	public async Task<ActionResult> GetChannelPointRewards()
	{
		var result = await Mediator.Send(new GetRewardsQuery());
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// Gets rewards from database, if empty, get rewards from Twitch
/// </summary>
public record GetChannelPointRewardsHandler(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest,
KoishibotDbContext Database
) : IRequestHandler<GetRewardsQuery, List<ChannelPointRewardVm>>
{
	public async Task<List<ChannelPointRewardVm>> Handle
	(GetRewardsQuery query, CancellationToken cancel)
	{
		var result = await Database.GetChannelPointRewards();
		if (result.Count == 0)
		{
			var streamerId = Settings.Value.StreamerTokens.UserId;
			var parameters = query.CreateParameters(streamerId);
			var response = await TwitchApiRequest.GetCustomRewards(parameters);
			var repoRewards = await Database.AddRewards(response);
			return repoRewards.ConvertToVm();
		}
		else
		{
			return result;
		}
	}
}

/*════════════════════【 QUERY 】════════════════════*/
public record GetRewardsQuery : IRequest<List<ChannelPointRewardVm>>
{
	public GetCustomRewardsParameters CreateParameters(string streamerId)
		=> new() { BroadcasterId = streamerId };
};