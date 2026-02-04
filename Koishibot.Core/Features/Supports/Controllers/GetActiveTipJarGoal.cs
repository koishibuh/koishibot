using Koishibot.Core.Features.Supports.Events;
using Koishibot.Core.Persistence;
using Microsoft.AspNetCore.Authorization;
namespace Koishibot.Core.Features.Supports.Controllers;


/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/goals")]
public class GetTipJarGoalController(KoishibotDbContext database) : ApiControllerBase
{
	[HttpGet("tipjar")]
	public async Task<ActionResult> GetTipJarGoal()
	{
		var result = await database.TipJarGoals
			.AsNoTracking()
			.Where(x => x.IsActive && x.EndedOn == null)
			.OrderByDescending(x => x.Id)
			.Select(x => new TipJarGoalVm(x.Title, x.CurrentAmount, x.GoalAmount))
			.FirstOrDefaultAsync();

		return result is null 
			? throw new Exception("No active goals found") 
			: Ok(result);
	}
}

/*══════════════════【 】══════════════════*/

public record TipJarGoalVm(string Title, int CurrentAmount, int GoalAmount);