using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.ChatCommands.Controllers;

// == ⚫ POST  == //

public class CreateChatCommandController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Commands"])]
	[HttpPost("/api/commands")]
	public async Task<ActionResult> CreateChatCommand
		([FromBody] CreateChatCommand e)
	{
		var result = await Mediator.Send(e);
		return Ok(result);
	}
}

// == ⚫ HANDLER  == //

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

// == ⚫ COMMAND  == //

public record CreateChatCommand
(
	string? Description,
	bool Enabled,
	string Message,
	PermissionLevel PermissionLevel,
	int UserCooldownSeconds,
	int GlobalCooldownSeconds,
	List<int> CommandNameIds,
	List<int>? TimerGroupIds
) : IRequest<int>
{
	public ChatCommand ConvertToModel()
	{
		return new ChatCommand
		{
			Description = Description ?? string.Empty,
			Enabled = Enabled,
			Message = Message,
			Permissions = PermissionLevel,
			UserCooldown = TimeSpan.FromMinutes(UserCooldownSeconds),
			GlobalCooldown = TimeSpan.FromMinutes(GlobalCooldownSeconds),
			CommandNames = CommandNameIds.Select(x => new CommandName { Id = x }).ToList(),
			TimerGroups = TimerGroupIds.Count > 0 ? TimerGroupIds.Select(x => new TimerGroup { Id = x }).ToList() : null
		};
	}
};


// == ⚫ VALIDATOR == //

public class CreateChatCommandValidator
		: AbstractValidator<CreateChatCommand>
{
	public KoishibotDbContext Database { get; }

	public CreateChatCommandValidator(KoishibotDbContext context)
	{
		Database = context;

		RuleFor(p => p.Message)
			.NotEmpty()
			.MaximumLength(500);

		RuleFor(p => p.PermissionLevel)
			.NotEmpty();

		RuleFor(p => p.CommandNameIds)
			.NotEmpty();
	}
}