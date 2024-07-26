//using Koishibot.Core.Features.ChannelPoints.Extensions;
//using Koishibot.Core.Features.ChannelPoints.Interfaces;
//using Koishibot.Core.Features.ChannelPoints.Models;
//using Koishibot.Core.Persistence;
//using Koishibot.Core.Services.TwitchEventSub.Extensions;
//namespace Koishibot.Core.Features.ChannelPoints;

//// == ⚫ GET == //

//public class ImportChannelPointRewardsController : ApiControllerBase
//{
//	[SwaggerOperation(Tags = new[] { "Point Reward" })]
//	[HttpGet("/api/point-reward/twitch")]
//	public async Task<ActionResult> ImportChannelPointRewardsFromTwitch()
//	{
//		var result = await Mediator.Send(new ImportRewardsQuery());
//		return Ok(result);
//	}
//}

//// == ⚫ QUERY == //

//public record ImportRewardsQuery()
//	: IRequest<List<ChannelPointRewardVm>>;


//// == ⚫ HANDLER == //

///// <summary>
///// Gets rewards from Twitch
///// </summary>
//public record ImportRewardsHandler(
//	IChannelPointsApi ChannelPointsApi,
//	KoishibotDbContext Database
//	)	: IRequestHandler<ImportRewardsQuery, List<ChannelPointRewardVm>>
//{
//	public async Task<List<ChannelPointRewardVm>> Handle
//		(ImportRewardsQuery c, CancellationToken cancel)
//	{
//		var result = await ChannelPointsApi.GetCustomRewards();

//		//var repoRewards = await result.AddAllToRepo(Database);
//		var repoRewards = await Database.AddRewards(result);

//		return repoRewards.ConvertToVm();
//	}
//}

//// == ⚫ TWITCH API == //

//public partial record ChannelPointsApi : IChannelPointsApi
//{
//	/// <summary>
//	/// <see href="	https://dev.twitch.tv/docs/api/reference/#get-custom-reward">Get Custom Rewards Documentation</see>
//	/// </summary>
//	/// <returns></returns>
//	public async Task<List<ChannelPointReward>> GetCustomRewards()
//	{
//		await TokenProcessor.EnsureValidToken();

//		var result = await TwitchApi.Helix.ChannelPoints.GetCustomRewardAsync(StreamerId);

//		return result is null || result.Data.Length == 0
//			? throw new Exception("Unable to get channel point rewards from Api")
//			: result.ConvertToEntity();
//	}
//}