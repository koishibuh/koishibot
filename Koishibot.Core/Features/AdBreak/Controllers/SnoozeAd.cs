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

// == ⚫ HANDLER == //

public record SnoozeAdHandler(
	IOptions<Settings> Settings,
	ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<SnoozeNextAdCommand, SnoozeNextAdDto>
{
	public string StreamerId = Settings.Value.StreamerTokens.UserId;

	public async Task<SnoozeNextAdDto> Handle
		(SnoozeNextAdCommand command, CancellationToken cancel)
	{
		var parameters = command.CreateParameters(StreamerId);

		var result = await TwitchApiRequest.SnoozeNextAd(parameters);
		return result.ConvertToDto();
	}
}

// == ⚫ QUERY == //

public record SnoozeNextAdCommand()
	: IRequest<SnoozeNextAdDto>
{
	public SnoozeNextAdRequestParameters CreateParameters(string streamerId)
		=> new SnoozeNextAdRequestParameters { BroadcasterId = streamerId };
};

// == ⚫ DTO == //

public record SnoozeNextAdDto(
	int AvailableSnoozeCount,
	DateTimeOffset? GainNextSnoozeAt,
	DateTimeOffset? NextAdScheduledAt
);