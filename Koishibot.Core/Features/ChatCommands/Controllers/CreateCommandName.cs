using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Persistence;

namespace Koishibot.Core.Features.ChatCommands.Controllers;

// TODO: FIX THIS
// /*══════════════════【 CONTROLLER 】══════════════════*/
// [Route("api/commands")]
// public class CreateCommandNameController : ApiControllerBase
// {
// 	[SwaggerOperation(Tags = ["Commands"])]
// 	[HttpPost("name")]
// 	public async Task<ActionResult> CreateCommandName
// 		([FromBody] CreateCommandNameCommand e)
// 	{
// 		var result = await Mediator.Send(e);
// 		return Ok(result);
// 	}
// }
//
// /*═══════════════════【 HANDLER 】═══════════════════*/
// /// <summary>
// /// 
// /// </summary>
// public record CreateCommandNameHandler(
// KoishibotDbContext Database
// ) : IRequestHandler<CreateCommandNameCommand, int>
// {
// 	public async Task<int> Handle
// 		(CreateCommandNameCommand c, CancellationToken cancel)
// 	{
// 		var commandName = c.ConvertToModel();
// 		var commandNameId = await Database.UpdateEntry(commandName);
// 		return commandNameId;
// 	}
// }
//
// /*═══════════════════【 COMMAND 】═══════════════════*/
// public record CreateCommandNameCommand(
// string Name
// ) : IRequest<int>
// {
// 	public CommandName ConvertToModel() => new() { Name = Name };
//
// 	public async Task<bool> IsCommandNameUnique(KoishibotDbContext database)
// 	{
// 		var result = await database.NewChatCommands
// 			.FirstOrDefaultAsync(p => p.Name == Name);
//
// 		return result is null;
// 	}
// }
//
// /*══════════════════【 VALIDATOR 】══════════════════*/
// public class CreateCommandNameValidator
// 	: AbstractValidator<CreateCommandNameCommand>
// {
// 	private KoishibotDbContext Database { get; }
//
// 	public CreateCommandNameValidator(KoishibotDbContext context)
// 	{
// 		Database = context;
//
// 		RuleFor(p => p.Name)
// 			.NotEmpty();
//
// 		RuleFor(p => p)
// 			.MustAsync(IsCommandNameUnique)
// 			.WithMessage("Command name already exists");
// 	}
//
// 	private async Task<bool> IsCommandNameUnique
// 		(CreateCommandNameCommand command, CancellationToken cancel)
// 		=> await command.IsCommandNameUnique(Database);
// }