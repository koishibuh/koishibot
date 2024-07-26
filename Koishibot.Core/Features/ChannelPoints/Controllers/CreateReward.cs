//using Koishibot.Core.Features.ChannelPoints.Extensions;
//using Koishibot.Core.Features.ChannelPoints.Interfaces;
//using Koishibot.Core.Features.ChannelPoints.Models;
//using Koishibot.Core.Persistence;
//using Koishibot.Core.Services.TwitchEventSub.Extensions;

//namespace Koishibot.Core.Features.ChannelPoints;

//// == ⚫ POST  == //

//public class CreateChannelPointRewardController : ApiControllerBase
//{
//	[SwaggerOperation(Tags = new[] { "Point Reward" })]
//	[HttpPost("/api/point-reward/twitch")]
//	public async Task<ActionResult> CreateChannelPointReward
//			([FromBody] CreateRewardCommand e)
//	{
//		var result = await Mediator.Send(e);
//		return Ok(result);
//	}
//}

//// == ⚫ COMMAND  == //

//public record CreateRewardCommand(
//	string Title,
//	string Prompt,
//	int Cost,
//	string BackgroundColor,
//	bool IsEnabled,
//	bool IsUserInputRequired,
//	bool IsMaxPerStreamEnabled,
//	int MaxPerStream,
//	bool IsMaxPerUserPerStreamEnabled,
//	int MaxPerUserPerStream,
//	bool IsGlobalCooldownEnabled,
//	int GlobalCooldownSeconds,
//	bool ShouldRedemptionsSkipRequestQueue
//	) : IRequest<int>
//{
//	public CreateCustomRewardsRequest CreateCustomRewardRequest()
//	{
//		return new CreateCustomRewardsRequest
//		{
//			Title = Title,
//			Cost = Cost,
//			IsEnabled = IsEnabled,
//			IsUserInputRequired = IsUserInputRequired,
//			Prompt = Prompt,
//			BackgroundColor = BackgroundColor,
//			IsMaxPerStreamEnabled = IsMaxPerStreamEnabled,
//			MaxPerStream = MaxPerStream,
//			IsMaxPerUserPerStreamEnabled = IsMaxPerUserPerStreamEnabled,
//			MaxPerUserPerStream = MaxPerUserPerStream,
//			IsGlobalCooldownEnabled = IsGlobalCooldownEnabled,
//			GlobalCooldownSeconds = GlobalCooldownSeconds,
//			ShouldRedemptionsSkipRequestQueue = ShouldRedemptionsSkipRequestQueue
//		};
//	}

//	public async Task<bool> IsRewardNameUnique(KoishibotDbContext database)
//	{
//		var result = await database.ChannelPointRewards
//			.FirstOrDefaultAsync(p => p.Title == Title);

//		return result is null;
//	}
//}

//// == ⚫ HANDLER  == //

///// <summary>
///// Takes request from client
///// Sends to API via ChannelPointsApi
///// Saves result to DB
///// https://dashboard.twitch.tv/u/elysiagriffin/viewer-rewards/channel-points/rewards
///// </summary>
//public record CreateRewardHandler(
//	IChannelPointsApi ChannelPointsApi,
//	KoishibotDbContext Database
//	)	: IRequestHandler<CreateRewardCommand, int>
//{
//	public async Task<int> Handle
//		(CreateRewardCommand c, CancellationToken cancel)
//	{
//		var reward = await ChannelPointsApi.CreateCustomReward(c);
//		return await Database.UpdateReward(reward);
//		// TODO: Return VM?
//	}
//}

//// == ⚫ VALIDATOR == //

//public class CreateRewardValidator
//		: AbstractValidator<CreateRewardCommand>
//{
//	public KoishibotDbContext Database { get; }

//	public CreateRewardValidator(KoishibotDbContext context)
//	{
//		Database = context;

//		RuleFor(p => p.Title)
//			.NotEmpty();

//		RuleFor(p => p)
//				.MustAsync(IsRewardNameUnique)
//				.WithMessage("Reward name already exists");
//	}

//	private async Task<bool> IsRewardNameUnique
//			(CreateRewardCommand command, CancellationToken cancel)
//	{
//		return await command.IsRewardNameUnique(Database);
//	}
//}

//// == ⚫ TWITCH API == //

//public partial record ChannelPointsApi : IChannelPointsApi
//{
//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#create-custom-rewards">Create Custom Reward Documentation</see><br/>
//	/// Title 45 character max, unique
//	/// </summary>
//	/// <returns></returns>
//	public async Task<ChannelPointReward> CreateCustomReward(CreateRewardCommand dto)
//	{
//		await TokenProcessor.EnsureValidToken();

//		var reward = dto.CreateCustomRewardRequest();
//		var result = await TwitchApi.Helix.ChannelPoints.CreateCustomRewardsAsync(StreamerId, reward);

//		return result is null || result.Data.Length == 0
//			? throw new Exception("Unable to create channel point reward")
//			: result.ConvertToEntity();
//	}
//}