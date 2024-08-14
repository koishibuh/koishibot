using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.AdBreak.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/ads")]
public class StartAdController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Ad Schedule"])]
	[HttpPost("twitch")]
	public async Task<ActionResult> StartAd()
	{
		var result = await Mediator.Send(new StartAdCommand());
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record StartAdHandler(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<StartAdCommand, StartAdDto>
{
	public async Task<StartAdDto> Handle
	(StartAdCommand command, CancellationToken cancel)
	{
		var streamerId = Settings.Value.StreamerTokens.UserId;
		// TODO: Have the length of ad change from UI
		var requestBody = command.CreateRequest(streamerId, AdLength.ThreeMinutes);

		var result = await TwitchApiRequest.StartAd(requestBody);
		return result.ConvertToDto();
	}
}

/*════════════════════【 COMMAND 】════════════════════*/
public record StartAdCommand : IRequest<StartAdDto>
{
	public StartAdRequestBody CreateRequest(string streamerId, AdLength length)
		=> new() { BroadcasterId = streamerId, AdLength = length };
};

/*════════════════════【 DTO 】════════════════════*/
public record StartAdDto(
int AdLength,
string? Message,
int AdCooldownSeconds
);