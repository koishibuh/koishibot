using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.StreamInformation.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/stream/summary")]
public class GetStreamSummaryController : ApiControllerBase
{
	[HttpGet]
	public async Task<ActionResult> GetStreamSummary()
	{
		var result = await Mediator.Send(new GetStreamSummaryQuery());
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetStreamSummaryHandler(
IAppCache Cache,
KoishibotDbContext Database
) : IRequestHandler<GetStreamSummaryQuery, StreamSummaryVm>
{
	public async Task<StreamSummaryVm> Handle(GetStreamSummaryQuery query, CancellationToken cancel)
	{
		var result = Cache.GetCurrentStreamSessionSummary();
		if (result is null)
		{
			var currentSession = await Database.GetRecentStreamSession();
			if (currentSession is null) throw new CustomException("StreamSession is null");
			
			return new StreamSummaryVm(currentSession.Id, currentSession.Summary);
		}
		return new StreamSummaryVm(result.StreamSessionId ?? 0, result.Summary);
		// Todo: Have this return today's support & dandle results
	}
}

/*════════════════════【 QUERY 】════════════════════*/
public record GetStreamSummaryQuery : IRequest<StreamSummaryVm>;
