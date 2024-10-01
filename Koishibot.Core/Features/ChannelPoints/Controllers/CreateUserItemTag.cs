using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.TwitchApi.Models;
using Koishibot.Core.Services.Wordpress;
using Koishibot.Core.Services.Wordpress.Responses;
namespace Koishibot.Core.Features.ChannelPoints.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/wordpress")]
public class CreateUserItemTagController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Wordpress"])]
	[HttpPost("item-tag")]
	public async Task<ActionResult> CreateUserItemTag
		([FromBody] CreateUserItemTagCommand e)
	{
		await Mediator.Send(e);
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record CreateUserItemTagHandler(
KoishibotDbContext Database,
ITwitchApiRequest TwitchApiRequest,
IWordpressService WordpressService
) : IRequestHandler<CreateUserItemTagCommand>
{
	public async Task Handle(CreateUserItemTagCommand command, CancellationToken cancel)
	{
		var user = await Database.GetUserByLogin(command.Username);
		if (user.NotInDatabase())
		{
			var parameters = command.CreateParameters();
			var result = await TwitchApiRequest.GetUsers(parameters);
			if (result.IsEmpty()) throw new CustomException("GetUsers was empty");

			user = result[0].CreateTwitchUser();
			await Database.UpdateEntry(user);
		}

		var itemTag = await Database.GetItemTagByUserId(user.Id);
		if (itemTag.NotInDatabase())
		{
			var wpItemTag = await WordpressService.GetItemTagByName(user.Name);
			if (wpItemTag is null)
			{
				wpItemTag = await WordpressService.CreateItemTag(user.Name);
				itemTag = wpItemTag.CreateEntity(user);
				await Database.UpdateEntry(itemTag);
			}
			else
			{
				itemTag = wpItemTag.CreateEntity(user);
				await Database.UpdateEntry(itemTag);
			}
		}
		else
		{
			throw new CustomException("User already has Wordpress Tag");
		}
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record CreateUserItemTagCommand(string Username) : IRequest
{
	public GetUsersRequestParameters CreateParameters() =>
		new() { UserLogins = new List<string> { Username.ToLower() } };
}