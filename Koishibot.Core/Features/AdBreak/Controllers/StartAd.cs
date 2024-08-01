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

// == ⚫ HANDLER == //

public record StartAdHandler(
	IOptions<Settings> Settings,
	ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<StartAdCommand, StartAdDto>
{
	public string StreamerId = Settings.Value.StreamerTokens.UserId;

	public async Task<StartAdDto> Handle
		(StartAdCommand command, CancellationToken cancel)
	{
		// Todo: Have the length of ad change from UI
		var requestBody = command.CreateRequest(StreamerId, AdLength.ThreeMinutes);

		var result = await TwitchApiRequest.StartAd(requestBody);
		return result.ConvertToDto();
	}
}

// == ⚫ COMMAND == //

public record StartAdCommand()
	: IRequest<StartAdDto>
{
	public StartAdRequestBody CreateRequest(string streamerId, AdLength length)
		=> new StartAdRequestBody { BroadcasterId = streamerId, AdLength = length };
};

// == ⚫ DTO == //

public record StartAdDto(
	int AdLength,
	string? Message,
	int AdCooldownSeconds
	);
