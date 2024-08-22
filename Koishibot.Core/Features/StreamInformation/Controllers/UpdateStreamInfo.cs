using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.StreamInformation.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/stream-info")]
public class UpdateStreamInfoController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Stream Info"])]
	[HttpPost("twitch")]
	public async Task<ActionResult> UpdateStreamInfo
	([FromBody] UpdateStreamInfoCommand command)
	{
		await Mediator.Send(command);
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record UpdateStreamInfoHandler(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<UpdateStreamInfoCommand>
{
	public async Task Handle
	(UpdateStreamInfoCommand command, CancellationToken cancel)
	{
		var parameters = command.CreateRequestParameters(Settings);
		var body = command.CreateRequestBody();

		await TwitchApiRequest.EditChannelInfo(parameters, body);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record UpdateStreamInfoCommand(
string StreamTitle,
string? CategoryId
) : IRequest
{
	public EditChannelInfoRequestParameters CreateRequestParameters(IOptions<Settings> settings) =>
		new() { BroadcasterId = settings.Value.StreamerTokens.UserId };

	public EditChannelInfoRequestBody CreateRequestBody() =>
		new() { Title = StreamTitle, GameId = CategoryId };
}