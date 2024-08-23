using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Persistence;

namespace Koishibot.Core.Features.ChatCommands.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/commands")]
public class CreateChatCommandController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Commands"])]
	[HttpPost]
	public async Task<ActionResult> CreateChatCommand
	([FromBody] CreateChatCommandRequest e)
	{
		var result = await Mediator.Send(e.Request);
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// 
/// </summary>
public record CreateChatCommandHandler(
IOptions<Settings> Settings,
KoishibotDbContext Database
) : IRequestHandler<CreateChatCommand, int>
{
	public async Task<int> Handle
	(CreateChatCommand c, CancellationToken cancel)
	{
		var command = c.ConvertToModel();
		var commandId = await Database.UpdateEntry(command);

		return commandId;
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record CreateChatCommandRequest(
CreateChatCommand Request);

public record CreateChatCommand(
List<CommandNameVm> CommandNames,
string? Description,
bool Enabled,
string Message,
string PermissionLevel,
int UserCooldownMinutes,
int GlobalCooldownMinutes,
List<int>? TimerGroupIds
) : IRequest<int>
{
	public ChatCommand ConvertToModel()
	{
		var timerGroups = TimerGroupIds?
		.Select(x => new TimerGroup { Id = x })
		.ToList();

		return new ChatCommand
		{
		Description = Description ?? string.Empty,
		Enabled = Enabled,
		Message = Message,
		Permissions = PermissionLevel,
		UserCooldown = TimeSpan.FromMinutes(UserCooldownMinutes),
		GlobalCooldown = TimeSpan.FromMinutes(GlobalCooldownMinutes),
		CommandNames = CommandNames.Select(x => new CommandName { Id = x.Id, Name = x.Name }).ToList(),
		TimerGroups = timerGroups
		};
	}
};

/*══════════════════【 VALIDATOR 】══════════════════*/
public class CreateChatCommandRequestValidator
: AbstractValidator<CreateChatCommandRequest>
{
	public KoishibotDbContext Database { get; }

	public CreateChatCommandRequestValidator(KoishibotDbContext context)
	{
		Database = context;

		RuleFor(p => p.Request.Message)
		.NotEmpty()
		.MaximumLength(500);

		RuleFor(p => p.Request.PermissionLevel)
		.NotEmpty();

		RuleFor(p => p.Request.CommandNames)
		.NotEmpty();
	}
}