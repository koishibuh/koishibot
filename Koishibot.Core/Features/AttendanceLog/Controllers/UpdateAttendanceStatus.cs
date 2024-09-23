using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.AttendanceLog.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/attendance")]
public class UpdateAttendanceStatus : ApiControllerBase
{
	[HttpPost("status")]
	public async Task<ActionResult> UpdateStatus([FromBody] UpdateAttendanceStatusCommand e)
	{
		await Mediator.Send(e);
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record EnableLightsHandler(
IAppCache Cache,
KoishibotDbContext Database
) : IRequestHandler<UpdateAttendanceStatusCommand>
{
	public async Task Handle(UpdateAttendanceStatusCommand command, CancellationToken cancel)
	{
		// TODO: if this is updated in the middle of stream

		var currentSession = await Database.GetRecentStreamSession();
		if (currentSession is null) throw new CustomException("StreamSession is null");

		currentSession.AttendanceMandatory = command.status;
		await Database.UpdateEntry(currentSession);

		var status = command.status == true ? Status.Online : Status.Offline;

		await Cache.UpdateServiceStatus(ServiceName.Attendance, status);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record UpdateAttendanceStatusCommand(
bool status) : IRequest;