using Koishibot.Core.Features.ChannelPoints.Extensions;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.ChannelPoints.Controllers;

// == ⚫ GET == //

public class GetChannelPointRewardsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Point Reward"])]
	[HttpGet("/api/point-rewards")]
	public async Task<ActionResult> GetChannelPointRewards()
	{
		var result = await Mediator.Send(new GetRewardsQuery());
		return Ok(result);
	}
}

// == ⚫ HANDLER == //

/// <summary>
/// Gets rewards from database, if empty, get rewards from Twitch
/// </summary>
public record GetChannelPointRewardsHandler(
	IOptions<Settings> Settings,
	ITwitchApiRequest TwitchApiRequest,
	KoishibotDbContext Database
	) : IRequestHandler<GetRewardsQuery, List<ChannelPointRewardVm>>
{
	public string StreamerId => Settings.Value.StreamerTokens.UserId;

	public async Task<List<ChannelPointRewardVm>> Handle
		(GetRewardsQuery query, CancellationToken cancel)
	{
		var result = await Database.GetChannelPointRewards();
		if (result is null)
		{
			var parameters = query.CreateParameters(StreamerId);
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

// == ⚫ QUERY == //

public record GetRewardsQuery(
	) : IRequest<List<ChannelPointRewardVm>>
{
	public GetCustomRewardsParameters CreateParameters(string streamerId)
		=> new GetCustomRewardsParameters { BroadcasterId = streamerId };
};
