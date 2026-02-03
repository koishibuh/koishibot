using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Persistence;
using Microsoft.AspNetCore.Authorization;
namespace Koishibot.Core.Features.Supports.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/goals")]
public class CreateTipJarGoalController(KoishibotDbContext database)  : ApiControllerBase
{
	[HttpPost("tipjar")]
	public async Task<ActionResult> CreateTipJarGoal
		([FromBody] TipJarVm data)
	{
		var goal = new TipJarGoal
		{
			Title = data.Title,
			CurrentAmount = 0,
			GoalAmount = data.Amount,
			IsActive = false
		};

		var result = await database.HasActiveGoal();
		if (result is false)
		{
			goal.StartedOn = DateTimeOffset.UtcNow;
			goal.IsActive = true;
		}
		
		await database.AddEntry(goal);
		
		// if is active, notify overlay to update goal
		return Ok();
	}
}
/*═══════════════════【 】═══════════════════*/
public record TipJarVm(
string Title,
int Amount
);