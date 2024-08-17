namespace Koishibot.Core.Features.Supports.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/stream-elements")]
public class SaveStreamElementsTokenController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["StreamElements"])]
	[HttpPost("token")]
	public async Task<ActionResult> SaveStreamElementsToken
		([FromBody] SaveStreamElementsTokenCommand command)
	{
		await Mediator.Send(command);
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record SaveStreamElementsTokenHandler(
IOptions<Settings> Settings
) : IRequestHandler<SaveStreamElementsTokenCommand>
{
	public Task Handle
	(SaveStreamElementsTokenCommand command, CancellationToken cancel)
	{
		Settings.Value.StreamElementsJwtToken = command.JwtToken;
		return Task.CompletedTask;
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record SaveStreamElementsTokenCommand(
string JwtToken
) : IRequest;