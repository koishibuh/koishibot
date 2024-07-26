using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.AdBreak.Controllers;

// == ⚫ GET == //

public class GetAdScheduleController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Ad Schedule"])]
	[HttpGet("/api/ads/twitch")]
	public async Task<ActionResult> GetAdSchedule()
	{
		var result = await Mediator.Send(new GetAdScheduleCommand());
		return Ok(result);
	}
}

// == ⚫ QUERY == //

public record GetAdScheduleCommand()
	: IRequest<AdScheduleDto>;

// == ⚫ HANDLER == //

public record GetAdScheduleHandler(
	IOptions<Settings> Settings,
	ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<GetAdScheduleCommand, AdScheduleDto>
{
	public async Task<AdScheduleDto> Handle
		(GetAdScheduleCommand command, CancellationToken cancel)
	{
		var parameters = new GetAdScheduleRequestParameters
		{ BroadcasterId = Settings.Value.StreamerTokens.UserId };

		var result = await TwitchApiRequest.GetAdSchedule(parameters);
		return result.ConvertToDto();
	}
}

// == ⚫ DTO == //

public record AdScheduleDto(
	int AvailableSnoozeCount,
	DateTimeOffset GainNextSnoozeAt,
	DateTimeOffset NextAdScheduledAt,
	TimeSpan AdDurationInSeconds,
	DateTimeOffset LastAdPlayedAt,
	int RemainingPrerollFreeTimeInSeconds
	)
{
	/// <summary>
	/// Offset by 1 minute 
	/// </summary>
	public TimeSpan CalculateAdjustedTimeUntilNextAd()
	{
		return NextAdScheduledAt - (DateTimeOffset.UtcNow - TimeSpan.FromMinutes(1));	
	}
}