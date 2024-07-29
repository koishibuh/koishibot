using Koishibot.Core.Features.TwitchUsers.Extensions;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.TwitchUsers.Controller;

// == ⚫ POST == //

public class AddTwitchUserController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Twitch User"])]
	[HttpPost("/api/twitch-user")]
	public async Task<ActionResult> AddTwitchUser
			([FromBody] AddTwitchUserCommand command)
	{
		await Mediator.Send(command);
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record AddTwitchUserCommand(string Username) : IRequest
{
	public async Task<bool> IsUserUnique(KoishibotDbContext database)
	{
		var result = await database.Users
				.FirstOrDefaultAsync(x => x.Name == Username);

		return result is null;
	}
};

// == ⚫ HANDLER == //

public record AddTwitchUserHandler(
		IOptions<Settings> Settings,
		ITwitchApiRequest TwitchApiRequest,
		KoishibotDbContext Database
		) : IRequestHandler<AddTwitchUserCommand>
{

	public async Task Handle
					(AddTwitchUserCommand c, CancellationToken cancel)
	{
		var parameters = new GetUsersRequestParameters
		{
			UserIds = new List<string> { c.Username }
		};

		var response = await TwitchApiRequest.GetUsers(parameters);
		// TODO: Check what happens if user doesnt exist

		var twitchUser = new TwitchUser().Set(response[0]);
		await Database.UpdateUser(twitchUser);
	}
}

// == ⚫ VALIDATOR == //

public class AddTwitchUserValidator
				: AbstractValidator<AddTwitchUserCommand>
{
	public KoishibotDbContext Database { get; set; }

	public AddTwitchUserValidator(KoishibotDbContext context)
	{
		Database = context;

		RuleFor(p => p.Username)
				.NotEmpty();

		RuleFor(p => p)
						.MustAsync(TwitchUserUnique)
						.WithMessage("Twitch User already exists");
	}

	private async Task<bool> TwitchUserUnique
					(AddTwitchUserCommand command, CancellationToken cancel)
	{
		return await command.IsUserUnique(Database);
	}
}