using Koishibot.Core.Persistence;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.ChannelPoints;

// == ⚫ POST  == //

public class CreateChannelPointRewardController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Point Reward"])]
	[HttpPost("/api/point-reward/twitch")]
	public async Task<ActionResult> CreateChannelPointReward
		([FromBody] CreateRewardCommand e)
	{
		await Mediator.Send(e);
		return Ok();
	}
}

// == ⚫ HANDLER  == //

/// <summary>
/// Takes request from client
/// Sends to API via ChannelPointsApi
/// Saves result to DB
/// https://dashboard.twitch.tv/u/elysiagriffin/viewer-rewards/channel-points/rewards
/// </summary>
public record CreateRewardHandler(
	IOptions<Settings> Settings,
	KoishibotDbContext Database,
	ITwitchApiRequest TwitchApiRequest
	) : IRequestHandler<CreateRewardCommand>
{
	public string StreamerId => Settings.Value.StreamerTokens.UserId;

	public async Task Handle
		(CreateRewardCommand command, CancellationToken cancel)
	{
		var parameters = command.CreateParameters(StreamerId);
		var body = command.CreateRequestBody();

		await TwitchApiRequest.CreateCustomReward(parameters, body);

		// Get confirmation from RewardCreated EventSub
	}
}

// == ⚫ COMMAND  == //

public record CreateRewardCommand(
	string Title,
	string Prompt,
	int Cost,
	string BackgroundColor,
	bool IsEnabled,
	bool IsUserInputRequired,
	bool IsMaxPerStreamEnabled,
	int MaxPerStream,
	bool IsMaxPerUserPerStreamEnabled,
	int MaxPerUserPerStream,
	bool IsGlobalCooldownEnabled,
	int GlobalCooldownSeconds,
	bool ShouldRedemptionsSkipRequestQueue
	) : IRequest
{
	public CreateCustomRewardRequestBody CreateRequestBody()
	{
		return new CreateCustomRewardRequestBody
		{
			Title = Title,
			Cost = Cost,
			IsEnabled = IsEnabled,
			IsUserInputRequired = IsUserInputRequired,
			Prompt = Prompt,
			BackgroundColor = BackgroundColor,
			IsMaxPerStreamEnabled = IsMaxPerStreamEnabled,
			MaxPerStream = MaxPerStream,
			IsMaxPerUserPerStreamEnabled = IsMaxPerUserPerStreamEnabled,
			MaxPerUserPerStream = MaxPerUserPerStream,
			IsGlobalCooldownEnabled = IsGlobalCooldownEnabled,
			GlobalCooldownSeconds = GlobalCooldownSeconds,
			ShouldRedemptionsSkipRequestQueue = ShouldRedemptionsSkipRequestQueue
		};
	}

	public async Task<bool> IsRewardNameUnique(KoishibotDbContext database)
	{
		var result = await database.ChannelPointRewards
			.FirstOrDefaultAsync(p => p.Title == Title);

		return result is null;
	}

	public CreateCustomRewardRequestParameters CreateParameters(string streamerId) 
		=> new CreateCustomRewardRequestParameters{ BroadcasterId = streamerId };
}

// == ⚫ VALIDATOR == //

public class CreateRewardValidator
		: AbstractValidator<CreateRewardCommand>
{
	public KoishibotDbContext Database { get; }

	public CreateRewardValidator(KoishibotDbContext context)
	{
		Database = context;

		RuleFor(p => p.Title)
			.NotEmpty();

		RuleFor(p => p)
				.MustAsync(IsRewardNameUnique)
				.WithMessage("Reward name already exists");
	}

	private async Task<bool> IsRewardNameUnique
			(CreateRewardCommand command, CancellationToken cancel)
	{
		return await command.IsRewardNameUnique(Database);
	}
}