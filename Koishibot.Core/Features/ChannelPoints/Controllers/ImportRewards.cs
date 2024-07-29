using Koishibot.Core.Features.ChannelPoints.Extensions;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.ChannelPoints;

// == ⚫ GET == //

public class ImportChannelPointRewardsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Point Reward"])]
	[HttpGet("/api/point-reward/twitch")]
	public async Task<ActionResult> ImportChannelPointRewardsFromTwitch()
	{
		var result = await Mediator.Send(new ImportRewardsQuery());
		return Ok(result);
	}
}

// == ⚫ HANDLER == //

/// <summary>
/// Gets rewards from Twitch
/// </summary>
public record ImportRewardsHandler(
	IOptions<Settings> Settings,
	ITwitchApiRequest TwitchApiRequest,
	KoishibotDbContext Database
	) : IRequestHandler<ImportRewardsQuery, List<ChannelPointRewardVm>>
{
	public string StreamerId => Settings.Value.StreamerTokens.UserId;

	public async Task<List<ChannelPointRewardVm>> Handle
		(ImportRewardsQuery query, CancellationToken cancel)
	{
		var parameters = query.CreateParameters(StreamerId);
		var result = await TwitchApiRequest.GetCustomRewards(parameters);
		var repoRewards = await Database.AddRewards(result);
		return repoRewards.ConvertToVm();
	}
}

// == ⚫ QUERY == //

public record ImportRewardsQuery(
	) : IRequest<List<ChannelPointRewardVm>>
{
	public GetCustomRewardsParameters CreateParameters(string streamerId) 
		=> new GetCustomRewardsParameters { BroadcasterId = streamerId };
};
