using Koishibot.Core.Persistence;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.TwitchUsers.Controller;


/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/twitch-user")]
public class GetTwitchUserIdController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Twitch User"])]
	[HttpGet("twitch")]
	public async Task<ActionResult> GetTwitchUserId
	([FromQuery] string username)
	{
		var command = new GetTwitchUserIdCommand(username);
		var result = await Mediator.Send(command);
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetTwitchUserIdHandler(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest,
KoishibotDbContext Database
) : IRequestHandler<GetTwitchUserIdCommand, TwitchUserIdVm>
{
	public async Task<TwitchUserIdVm> Handle
	(GetTwitchUserIdCommand command, CancellationToken cancel)
	{
		var parameters = command.CreateParameters();
		var response = await TwitchApiRequest.GetUsers(parameters);


		return new TwitchUserIdVm(response[0].Id, response[0].Login, response[0].Name);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record GetTwitchUserIdCommand(
string Username
) : IRequest<TwitchUserIdVm>
{
	public GetUsersRequestParameters CreateParameters()
		=> new() { UserLogins = new List<string> { Username.ToLower() }};
};

// /*══════════════════【 VALIDATOR 】══════════════════*/
// public class GetTwitchUserIdValidator
// : AbstractValidator<GetTwitchUserIdCommand>
// {
// 	private KoishibotDbContext Database { get; set; }
//
// 	public GetTwitchUserIdValidator(KoishibotDbContext context)
// 	{
// 		Database = context;
//
// 		RuleFor(p => p.Username)
// 		.NotEmpty();
// 	}
// }

/*══════════════════【 RESPONSE 】══════════════════*/
public record TwitchUserIdVm(
string Id,
string Login,
string Name
);