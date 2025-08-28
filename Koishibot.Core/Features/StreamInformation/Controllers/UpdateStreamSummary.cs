using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.StreamInformation.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/stream/summary")]
public class UpdateStreamSummaryController : ApiControllerBase
{
	[HttpPatch]
	public async Task<ActionResult> UpdateStreamSummary
		([FromBody] UpdateStreamSummaryCommand command)
	{
		await Mediator.Send(command);
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record UpdateStreamSummaryHandler(
IOptions<Settings> Settings,
IAppCache Cache,
KoishibotDbContext Database
) : IRequestHandler<UpdateStreamSummaryCommand>
{
	public async Task Handle
		(UpdateStreamSummaryCommand command, CancellationToken cancel)
	{
		var currentSession = await Database.GetRecentStreamSession();
		if (currentSession is null) throw new CustomException("StreamSession is null");
		if (currentSession.Id == command.Id)
		{
			currentSession.Summary = command.Summary;
			await Database.UpdateEntry(currentSession);

			Cache.UpdateCurrentStreamSessionSummary(command.Summary);
		}

		// TODO: If Id doesn't match what do
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record UpdateStreamSummaryCommand(int Id, string Summary) : IRequest;

public record StreamSummaryVm(int Id, string Summary);