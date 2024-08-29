using Koishibot.Core.Services.TwitchApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace Koishibot.Core.Features.Shoutouts.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/shoutout")]
public class CreateShoutoutController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Shoutout"])]
	[HttpPost]
	public async Task<ActionResult> CreateShoutout([FromBody] CreateShoutoutCommand command)
	{
		await Mediator.Send(command);
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record CreateShoutoutHandler(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<CreateShoutoutCommand>
{
	public async Task Handle(CreateShoutoutCommand command, CancellationToken cancel)
	{
		var streamerId = Settings.Value.StreamerTokens.UserId;

		var parameters = command.CreateParameters(streamerId);
		await TwitchApiRequest.SendShoutout(parameters);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record CreateShoutoutCommand(
string ToStreamerId
) : IRequest
{
	public SendShoutoutParameters CreateParameters(string streamerId) =>
	new() {
	FromBroadcasterId = streamerId,
	ToBroadcasterId = ToStreamerId,
	ModeratorId = streamerId
	};
};