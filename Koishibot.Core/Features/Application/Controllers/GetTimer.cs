using Koishibot.Core.Features.AdBreak.Extensions;
using Koishibot.Core.Features.Common.Models;

namespace Koishibot.Core.Features.Application.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/timer")]
public class GetTimerController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Timer"])]
	[HttpGet]
	public async Task<ActionResult> GetTimer()
	{
		await Mediator.Send(new GetTimerQuery());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetTimerHandler(
IAppCache Cache,
ISignalrService Signalr
) : IRequestHandler<GetTimerQuery>
{
	public async Task Handle
	(GetTimerQuery query, CancellationToken cancel)
	{
		var result = Cache.GetCurrentTimer();
		var timer = result.ConvertToVm();
		await Signalr.UpdateTimerOverlay(timer);
	}
}

/*════════════════════【 QUERY 】════════════════════*/
public record GetTimerQuery : IRequest;