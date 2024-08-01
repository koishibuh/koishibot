using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.ChatCommands.Controllers;

// == ⚫ GET == //

public class GetChatCommandsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Commands"])]
	[HttpGet("/api/commands")]
	public async Task<ActionResult> GetChatCommands()
	{
		var result = await Mediator.Send(new GetChatCommandsQuery());
		return Ok(result);
	}
}

// == ⚫ HANDLER == //

public record GetChatCommandsHandler(
	KoishibotDbContext Database
) : IRequestHandler<GetChatCommandsQuery, ChatCommandInfoVm>
{
	public async Task<ChatCommandInfoVm> Handle
		(GetChatCommandsQuery command, CancellationToken cancel)
	{
		return await Database.GetCommands();
	}
}

public static class GetChatCommandsExtension
{
	public static async Task<ChatCommandInfoVm> GetCommands(this KoishibotDbContext database)
	{
		var commandNames = await database.CommandNames
			.Where(x => x.ChatCommandId == null)
			.Select(x => new CommandNameVm(x.Id, x.Name))
			.ToListAsync();

		var commands = await database.ChatCommands
			.Include(x => x.CommandNames)
			.Select(x => new ChatCommandVm(x.Id, x.Description, x.Enabled, x.Message,
			x.Permissions.ToString(), x.UserCooldown, x.GlobalCooldown,
			x.CommandNames.Select(y => new CommandNameVm(y.Id, y.Name)).ToList())).ToListAsync();

		return new ChatCommandInfoVm(commandNames, commands);

	}
}


// == ⚫ QUERY == //

public record GetChatCommandsQuery()
	: IRequest<ChatCommandInfoVm>;


public record ChatCommandInfoVm (
	List<CommandNameVm> CommandNames,
	List<ChatCommandVm> Commands
	);

public record CommandNameVm(
	int Id,
	string Name
	);

public record ChatCommandVm(
	int Id,
	string Description,
	bool Enabled,
	string Message,
	string PermissionLevel,
	TimeSpan UserCooldown,
	TimeSpan GlobalCooldown,
	List<CommandNameVm> Names
	);