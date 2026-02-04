using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
namespace Koishibot.Core.Features.Supports.Controllers;


/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/goals")]
public class GetOverlayGoalsController(KoishibotDbContext database) : ApiControllerBase
{
	[HttpGet]
	public async Task<ActionResult> GetOverlayGoals()
	{
		var tipJar = await database.GetActiveTipJarGoalVm();
		
		if (tipJar is null)
			throw new Exception("No active goals found");
		
		var today = DateTimeOffset.UtcNow.Date;
		var tomorrow = today.AddDays(1);

		var result = await database.Subscriptions
			.Where(x => x.Timestamp >= today && x.Timestamp <= tomorrow)
			.CountAsync();

		var goal = new GoalVm(tipJar, new SubGoalVm(result));
		
		return Ok(goal);
	}
}

/*══════════════════【 】══════════════════*/

public record GoalVm(TipJarGoalVm tipJar, SubGoalVm subGoal);

public record SubGoalVm(int CurrentAmount);