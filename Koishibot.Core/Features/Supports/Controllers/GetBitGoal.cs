using Koishibot.Core.Features.Supports.Events;
using Koishibot.Core.Persistence;
using Microsoft.AspNetCore.Authorization;
namespace Koishibot.Core.Features.Supports.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/goals")]
public class GetBitGoalController(KoishibotDbContext database) : ApiControllerBase
{
	[HttpGet("bits")]
	public async Task<ActionResult> GetBitGoal()
	{
		// query database for bits recorded for today, update to use stream session
		// cache the bits
		var today = DateTimeOffset.UtcNow;
		var end = today.AddDays(1);
		
		var result = await database.Cheers
			.Where(x => x.Timestamp >= today && x.Timestamp <= end)
			.Select(x => x.BitsAmount)
			.ToListAsync();

		var goalEventVm = new GoalEventVm("Bits", result.Sum());
		
		return Ok(goalEventVm);
	}
}