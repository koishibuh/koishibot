using Koishibot.Core.Features.Supports.Events;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.Supports.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/goals")]
public class GetSubGoalController(KoishibotDbContext database) : ApiControllerBase
{
	[HttpGet("subs")]
	public async Task<ActionResult> GetSubGoal()
	{
		// query database for subs recorded for today

		var today = DateTimeOffset.UtcNow;
		var end = today.AddDays(1);
		
		var result = await database.Subscriptions
			.Where(x => x.Timestamp >= today && x.Timestamp <= end)
			.CountAsync();
		
		// cache?
		
		var goalEventVm = new GoalEventVm("Subs", result);
		
		return Ok(goalEventVm);
	}
}