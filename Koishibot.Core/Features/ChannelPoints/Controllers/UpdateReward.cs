//using Koishibot.Core.Features.ChannelPoints.Extensions;
//using Koishibot.Core.Features.ChannelPoints.Interfaces;
//using Koishibot.Core.Features.ChannelPoints.Models;
//using Koishibot.Core.Features.Common;
//using Koishibot.Core.Persistence;
//using Koishibot.Core.Services.TwitchEventSub.Extensions;
//using TwitchLib.Api.Helix.Models.ChannelPoints.UpdateCustomReward;
//namespace Koishibot.Core.Features.ChannelPoints;

//// == ⚫ PATCH  == //

//public class UpdateChannelPointRewardController : ApiControllerBase
//{
//	[SwaggerOperation(Tags = ["Point Reward"])]
//	[HttpPatch("/api/point-reward/twitch")]
//	public async Task<ActionResult> UpdateChannelPointReward
//			([FromBody] UpdateRewardCommand e)
//	{
//		await Mediator.Send(e);
//		return Ok();
//	}
//}

//// == ⚫ COMMAND  == //

//public record UpdateRewardCommand(
//	string TwitchId,
//	string? Title,
//	string? Prompt,
//	int? Cost,
//	string? BackgroundColor,
//	bool? IsEnabled,
//	bool? IsUserInputRequired,
//	bool? IsMaxPerStreamEnabled,
//	int? MaxPerStream,
//	bool? IsMaxPerUserPerStreamEnabled,
//	int? MaxPerUserPerStream,
//	bool? IsGlobalCooldownEnabled,
//	int? GlobalCooldownSeconds,
//	bool? IsPaused,
//	bool? ShouldRedemptionsSkipRequestQueue
//	) : IRequest
//{
//	public UpdateCustomRewardRequest ConvertToRequest()
//	{
//		return new UpdateCustomRewardRequest
//		{
//			Title = Title,
//			Prompt = Prompt,
//			Cost = Cost,
//			BackgroundColor = BackgroundColor,
//			IsEnabled = IsEnabled,
//			IsUserInputRequired = IsUserInputRequired,
//			IsMaxPerStreamEnabled = IsMaxPerStreamEnabled,
//			MaxPerStream = MaxPerStream,
//			IsMaxPerUserPerStreamEnabled = IsMaxPerUserPerStreamEnabled,
//			MaxPerUserPerStream = MaxPerUserPerStream,
//			IsGlobalCooldownEnabled = IsGlobalCooldownEnabled,
//			GlobalCooldownSeconds = GlobalCooldownSeconds,
//			IsPaused = IsPaused,
//			ShouldRedemptionsSkipRequestQueue = ShouldRedemptionsSkipRequestQueue
//		};
//	}
//};

//// == ⚫ HANDLER  == //

//public record UpdateRewardHandler(
//	IChannelPointsApi ChannelPointsApi,
//	KoishibotDbContext Database
//	) : IRequestHandler<UpdateRewardCommand>
//{
//	public async Task Handle(UpdateRewardCommand c, CancellationToken cancel)
//	{
//		var result = await ChannelPointsApi.UpdateCustomReward(c);
//		await Database.UpdateReward(result);
//	}
//}

//// == ⚫ TWITCH API == //

//public partial record ChannelPointsApi : IChannelPointsApi
//{
//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-redemption-status">Update Redemption Status Documentation</see>
//	/// </summary>
//	/// <returns></returns>
//	public async Task<ChannelPointReward> UpdateCustomReward(UpdateRewardCommand reward)
//	{
//		await TokenProcessor.EnsureValidToken();

//		var rewardRequest = reward.ConvertToRequest();

//		var result = await TwitchApi.Helix.ChannelPoints
//			.UpdateCustomRewardAsync(StreamerId, reward.TwitchId, rewardRequest);

//		return result.Data.Length == 0
//			? throw new Exception("Unable to update channel point reward from Api")
//			: result.Data[0].ConvertToEntity();
//	}
//}