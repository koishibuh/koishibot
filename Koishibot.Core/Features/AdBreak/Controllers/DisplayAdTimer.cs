using Koishibot.Core.Features.AdBreak.Models;
using Microsoft.AspNetCore.Authorization;
namespace Koishibot.Core.Features.AdBreak.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/ads")]
public class DisplayAdTimerController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Ad Schedule"])]
	[HttpPost("test")]
	public async Task<ActionResult> DisplayAdTimer
	([FromBody] DisplayAdTimerCommand command)
	{
		await Mediator.Send(command);
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record DisplayAdTimerHandler(
ISignalrService Signalr
) : IRequestHandler<DisplayAdTimerCommand>
{
	public async Task Handle
		(DisplayAdTimerCommand command, CancellationToken cancel)
	{
		var adBreakVm = new AdBreakVm(command.Milliseconds, DateTime.Now);
		await Signalr.SendAdStartedEvent(adBreakVm);
	}
}

/*════════════════════【 COMMAND 】════════════════════*/
public record DisplayAdTimerCommand(
int Seconds
) : IRequest
{
	public int Milliseconds => Seconds * 1000;
};