using Koishibot.Core.Persistence;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.ChannelPoints;

// == ⚫ PATCH  == //

public class UpdateChannelPointRewardController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Point Reward"])]
	[HttpPatch("/api/point-rewards/twitch")]
	public async Task<ActionResult> UpdateChannelPointReward
			([FromBody] UpdateRewardCommand e)
	{
		await Mediator.Send(e);
		return Ok();
	}
}

// == ⚫ HANDLER  == //

public record UpdateRewardHandler(
		IOptions<Settings> Settings,
	KoishibotDbContext Database,
		ITwitchApiRequest TwitchApiRequest
	) : IRequestHandler<UpdateRewardCommand>
{
	public string StreamerId => Settings.Value.StreamerTokens.UserId;

	public async Task Handle(UpdateRewardCommand command, CancellationToken cancel)
	{

		var parameters = command.CreateParameters(StreamerId);
		var body = command.CreateRequestBody();

		await TwitchApiRequest.UpdateCustomReward(parameters, body);

		// Get confirmation from RewardUpdated EventSub
	}
}

// == ⚫ COMMAND  == //

public record UpdateRewardCommand(
	string TwitchId,
	string? Title,
	string? Prompt,
	int? Cost,
	string? BackgroundColor,
	bool? IsEnabled,
	bool? IsUserInputRequired,
	bool? IsMaxPerStreamEnabled,
	int? MaxPerStream,
	bool? IsMaxPerUserPerStreamEnabled,
	int? MaxPerUserPerStream,
	bool? IsGlobalCooldownEnabled,
	int? GlobalCooldownSeconds,
	bool? IsPaused,
	bool? ShouldRedemptionsSkipRequestQueue
	) : IRequest
{
	public UpdateCustomRewardRequestParameters CreateParameters(string streamerId)
		=> new UpdateCustomRewardRequestParameters { BroadcasterId = streamerId };

	public UpdateCustomRewardRequestBody CreateRequestBody()
	{
		return new UpdateCustomRewardRequestBody
		{
			Title = Title,
			Prompt = Prompt,
			Cost = Cost,
			BackgroundColor = BackgroundColor,
			IsEnabled = IsEnabled,
			IsUserInputRequired = IsUserInputRequired,
			IsMaxPerStreamEnabled = IsMaxPerStreamEnabled,
			MaxPerStream = MaxPerStream,
			IsMaxPerUserPerStreamEnabled = IsMaxPerUserPerStreamEnabled,
			MaxPerUserPerStream = MaxPerUserPerStream,
			IsGlobalCooldownEnabled = IsGlobalCooldownEnabled,
			GlobalCooldownSeconds = GlobalCooldownSeconds,
			IsPaused = IsPaused,
			ShouldRedemptionsSkipRequestQueue = ShouldRedemptionsSkipRequestQueue
		};
	}
};
