using Koishibot.Core.Persistence;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.ChannelPoints;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/point-rewards")]
public class UpdateChannelPointRewardController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Point Reward"])]
	[HttpPatch("twitch")]
	public async Task<ActionResult> UpdateChannelPointReward
	([FromBody] UpdateRewardCommand e)
	{
		await Mediator.Send(e);
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record UpdateRewardHandler(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<UpdateRewardCommand>
{
	public async Task Handle(UpdateRewardCommand command, CancellationToken cancel)
	{
		var streamerId = Settings.Value.StreamerTokens.UserId;
		var parameters = command.CreateParameters(streamerId);
		var body = command.CreateRequestBody();

		await TwitchApiRequest.UpdateCustomReward(parameters, body);
		// Get confirmation from RewardUpdated EventSub
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
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
		=> new() { BroadcasterId = streamerId };

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