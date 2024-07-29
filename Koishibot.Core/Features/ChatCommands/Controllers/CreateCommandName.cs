using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Persistence;

namespace Koishibot.Core.Features.ChatCommands.Controllers;

// == ⚫ POST  == //

public class CreateCommandNameController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Commands"])]
	[HttpPost("/api/commands/names")]
	public async Task<ActionResult> CreateCommandName
			([FromBody] CreateCommandNameCommand e)
	{
		var result = await Mediator.Send(e);
		return Ok(result);
	}
}

// == ⚫ HANDLER  == //

/// <summary>
/// 
/// </summary>
public record CreateCommandNameHandler(
	IOptions<Settings> Settings,
	KoishibotDbContext Database
	) : IRequestHandler<CreateCommandNameCommand, int>
{
	public async Task<int> Handle
		(CreateCommandNameCommand c, CancellationToken cancel)
	{
		var commandName = c.ConvertToModel();
		var commandNameId = await Database.UpdateEntry(commandName);

		return commandNameId;
	}
}

// == ⚫ COMMAND  == //

public record CreateCommandNameCommand(
	string Name
	) : IRequest<int>
{
	public CommandName ConvertToModel() => new(){ Name = Name };
}


// == ⚫ VALIDATOR == //

public class CreateCommandNameValidator
		: AbstractValidator<CreateCommandNameCommand>
{
	public KoishibotDbContext Database { get; }

	public CreateCommandNameValidator(KoishibotDbContext context)
	{
		Database = context;

		RuleFor(p => p.Name)
			.NotEmpty();

		RuleFor(p => p)
		.MustAsync(IsCommandNameUnique)
		.WithMessage("Command name already exists");
	}

	private async Task<bool> IsCommandNameUnique
			(CreateCommandNameCommand command, CancellationToken cancel)
	{
		return await command.IsCommandNameUnique(Database);
	}
}