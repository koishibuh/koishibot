using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.TwitchApi.Models;
using Koishibot.Core.Services.Wordpress;
using Koishibot.Core.Services.Wordpress.Responses;
using System.Text.RegularExpressions;

namespace Koishibot.Core.Features.ChannelPoints.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/dragon-quest")]
public class ImportDragonsFromSiteController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Dragon Quest"])]
	[HttpGet("wordpress")]
	public async Task<ActionResult> ImportDragonsFromSite()
	{
		await Mediator.Send(new ImportDragonsFromSiteCommand());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record ImportDragonsFromSiteHandler(
KoishibotDbContext Database,
IWordpressService WordpressService,
ITwitchApiRequest TwitchApiRequest,
ISignalrService SignalrService
) : IRequestHandler<ImportDragonsFromSiteCommand>
{
	public async Task Handle(ImportDragonsFromSiteCommand command, CancellationToken cancel)
	{
		var dragonsWp = await WordpressService.GetItems();

		foreach (var dragon in dragonsWp)
		{
			if (Database.DragonRecorded(dragon)) { continue; }

			var code = GetDragonCode(dragon.Content.Rendered);

			if (dragon.ItemTagIds.IsEmpty())
			{
				await SignalrService.SendError($"No tags found for Dragon {code}");
				continue;
			}

			var itemTagWordpress = await Database.GetItemTagByWordpressId(dragon.ItemTagIds[0]);
			if (itemTagWordpress.NotInDatabase())
			{
				var id = dragon.ItemTagIds[0].ToString();
				var response = await WordpressService.GetItemTagById(id);

				var user = await Database.FindUserByTwitchName(response.Name);
				if (user.NotInDatabase())
				{
					var userResponse = await GetUserFromTwitch(user!.Name);
					if (userResponse.IsEmpty())
					{
						await SignalrService.SendError($"No tags found for Dragon {response.Name}");
						continue;
					}

					user = await AddUserToDatabase(userResponse[0]);
				}

				itemTagWordpress = new WordpressItemTag(user!.Id, dragon.ItemTagIds[0]);
				await Database.UpdateEntry(itemTagWordpress);
			}

			var koiKinDragon = new KoiKinDragon
			{
				WordpressId = dragon.WordpressId,
				Timestamp = dragon.Date,
				Code = code,
				Name = dragon.Title.Rendered ?? "",
				ItemTagId = itemTagWordpress!.Id
			};

			await Database.UpdateEntry(koiKinDragon);
		}
	}

/*═══════════════════【】═══════════════════*/
	private async Task<List<UserData>> GetUserFromTwitch(string name)
	{
		var parameters = new GetUsersRequestParameters { UserLogins = [name.ToLower()] };
		return await TwitchApiRequest.GetUsers(parameters);
	}

	private async Task<TwitchUser> AddUserToDatabase(UserData user)
	{
		var twitchUser = new TwitchUser { TwitchId = user.Id, Login = user.Login, Name = user.Name, Permissions = PermissionLevel.Everyone };
		return await Database.UpdateEntryReturn(twitchUser);
	}

	private static string GetDragonCode(string content)
	{
		const string pattern = @"https://dragcave\.net/view/([a-zA-Z0-9]{5})";
		var match = Regex.Match(content, pattern);

		return match.Success
			? match.Groups[1].Value
			: "Unknown";
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record ImportDragonsFromSiteCommand : IRequest;