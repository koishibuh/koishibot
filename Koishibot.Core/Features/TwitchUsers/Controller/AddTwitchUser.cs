using Koishibot.Core.Features.TwitchUsers.Extensions;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.TwitchUsers.Controller;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/twitch-user")]
public class AddTwitchUserController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Twitch User"])]
	[HttpPost]
	public async Task<ActionResult> AddTwitchUser
	([FromBody] AddTwitchUserCommand command)
	{
		var result = await Mediator.Send(command);
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record AddTwitchUserHandler(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest,
KoishibotDbContext Database
) : IRequestHandler<AddTwitchUserCommand, int>
{
	public async Task<int> Handle
	(AddTwitchUserCommand command, CancellationToken cancel)
	{
		// var parameters = command.CreateParameters();
		// var response = await TwitchApiRequest.GetUsers(parameters);
		//
		// var twitchUser = new TwitchUser().Set(response[0]);
		var twitchUser = command.CreateEntity();
		await Database.UpdateUser(twitchUser);

		return twitchUser.Id;
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record AddTwitchUserCommand(
string Id,
string Login,
string Name
) : IRequest<int>
{
	public GetUsersRequestParameters CreateParameters()
		=> new() { UserLogins = new List<string> { Name.ToLower() } };

	public TwitchUser CreateEntity()
		=> new TwitchUser
		{
			TwitchId = Id,
			Login = Login,
			Name = Name
		};

	public async Task<bool> IsUserUnique(KoishibotDbContext database)
	{
		var result = await database.Users
		.FirstOrDefaultAsync(x => x.Name == Name);

		return result is null;
	}
};

/*══════════════════【 VALIDATOR 】══════════════════*/
public class AddTwitchUserValidator
: AbstractValidator<AddTwitchUserCommand>
{
	private KoishibotDbContext Database { get; set; }

	public AddTwitchUserValidator(KoishibotDbContext context)
	{
		Database = context;

		RuleFor(p => p.Name)
		.NotEmpty();

		RuleFor(p => p)
		.MustAsync(TwitchUserUnique)
		.WithMessage("Twitch User already exists");
	}

	private async Task<bool> TwitchUserUnique
	(AddTwitchUserCommand command, CancellationToken cancel)
		=> await command.IsUserUnique(Database);
}