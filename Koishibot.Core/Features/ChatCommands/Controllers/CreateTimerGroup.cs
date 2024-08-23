using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Persistence;

namespace Koishibot.Core.Features.ChatCommands.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/commands")]
public class CreateTimerGroupController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Commands"])]
	[HttpPost("timer-groups")]
	public async Task<ActionResult> CreateTimerGroup
	([FromBody] CreateTimerGroupCommand e)
	{
		var result = await Mediator.Send(e);
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// 
/// </summary>
public record CreateTimerGroupHandler(
KoishibotDbContext Database
) : IRequestHandler<CreateTimerGroupCommand, int>
{
	public async Task<int> Handle
	(CreateTimerGroupCommand c, CancellationToken cancel)
	{
		var timerGroup = c.ConvertToModel();
		var timerGroupId = await Database.UpdateEntry(timerGroup);
		return timerGroupId;
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record CreateTimerGroupCommand(
string Name,
string? Description,
TimeSpan Interval
) : IRequest<int>
{
	public TimerGroup ConvertToModel()
		=> new() {
		Name = Name,
		Description = Description ?? string.Empty,
		Interval = Interval
		};

	public async Task<bool> IsTimerGroupNameUnique(KoishibotDbContext database)
	{
		var result = await database.TimerGroups
		.FirstOrDefaultAsync(p => p.Name == Name);

		return result is null;
	}
}

/*══════════════════【 VALIDATOR 】══════════════════*/
public class TimerGroupValidator : AbstractValidator<CreateTimerGroupCommand>
{
	private KoishibotDbContext Database { get; }

	public TimerGroupValidator(KoishibotDbContext context)
	{
		Database = context;

		RuleFor(p => p.Name)
		.NotEmpty();

		RuleFor(p => p.Interval)
		.NotEmpty();

		RuleFor(p => p)
		.MustAsync(IsTimerGroupNameUnique)
		.WithMessage("Reward name already exists");
	}

	private async Task<bool> IsTimerGroupNameUnique
	(CreateTimerGroupCommand command, CancellationToken cancel)
		=> await command.IsTimerGroupNameUnique(Database);
}