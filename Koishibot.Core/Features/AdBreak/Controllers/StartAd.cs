using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.AdBreak.Controllers;

// == ⚫ POST == //

public class StartAdController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Ad Schedule"])]
	[HttpPost("/api/ads/twitch")]
	public async Task<ActionResult> StartAd()
	{
		var result = await Mediator.Send(new StartAdCommand());
		return Ok(result);
	}
}

// == ⚫ QUERY == //

public record StartAdCommand()
	: IRequest<StartAdDto>;

// == ⚫ HANDLER == //

public record StartAdHandler(
	IOptions<Settings> Settings,
	ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<StartAdCommand, StartAdDto>
{
	public async Task<StartAdDto> Handle
		(StartAdCommand command, CancellationToken cancel)
	{
		var requestBody = CreateRequest(AdLength.ThreeMinutes);

		var result = await TwitchApiRequest.StartAd(requestBody);
		return result.ConvertToDto();
	}

	public StartAdRequestBody CreateRequest(AdLength length)
	{
		return new StartAdRequestBody
		{
			BroadcasterId = Settings.Value.StreamerTokens.UserId,
			AdLength = length
		};
	}
}

// == ⚫ DTO == //

public record StartAdDto(
	int AdLength,
	string? Message,
	int AdCooldownSeconds
	);
