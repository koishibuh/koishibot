using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.AdBreak.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/ads")]
public class SnoozeNextAdController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Ad Schedule"])]
	[HttpPatch("twitch")]
	public async Task<ActionResult> SnoozeNextAd()
	{
		var result = await Mediator.Send(new SnoozeNextAdCommand());
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record SnoozeAdHandler(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<SnoozeNextAdCommand, SnoozeNextAdDto>
{
	public async Task<SnoozeNextAdDto> Handle
		(SnoozeNextAdCommand command, CancellationToken cancel)
	{
		var streamerId = Settings.Value.StreamerTokens.UserId;
		var parameters = command.CreateParameters(streamerId);

		var result = await TwitchApiRequest.SnoozeNextAd(parameters);
		return result.ConvertToDto();
	}
}

/*════════════════════【 COMMAND 】════════════════════*/
public record SnoozeNextAdCommand : IRequest<SnoozeNextAdDto>
{
	public SnoozeNextAdRequestParameters CreateParameters(string streamerId)
		=> new() { BroadcasterId = streamerId };
};

/*══════════════════════【 DTO 】══════════════════════*/
public record SnoozeNextAdDto(
int AvailableSnoozeCount,
DateTimeOffset? GainNextSnoozeAt,
DateTimeOffset? NextAdScheduledAt
);