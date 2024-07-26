using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.AdBreak.Controllers;

// == ⚫ PATCH == //

public class SnoozeNextAdController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Ad Schedule"])]
	[HttpPatch("/api/ads/twitch")]
	public async Task<ActionResult> SnoozeNextAd()
	{
		var result = await Mediator.Send(new SnoozeNextAdCommand());
		return Ok(result);
	}
}

// == ⚫ QUERY == //

public record SnoozeNextAdCommand()
	: IRequest<SnoozeNextAdDto>;

// == ⚫ HANDLER == //

public record SnoozeAdHandler(
	IOptions<Settings> Settings,
	ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<SnoozeNextAdCommand, SnoozeNextAdDto>
{
	public async Task<SnoozeNextAdDto> Handle
		(SnoozeNextAdCommand command, CancellationToken cancel)
	{
		var parameters = new SnoozeNextAdRequestParameters
		{ BroadcasterId = Settings.Value.StreamerTokens.UserId };

		var result = await TwitchApiRequest.SnoozeNextAd(parameters);
		return result.ConvertToDto();
	}
}

// == ⚫ DTO == //

public record SnoozeNextAdDto(
	int AvailableSnoozeCount,
	DateTimeOffset? GainNextSnoozeAt,
	DateTimeOffset? NextAdScheduledAt
	);