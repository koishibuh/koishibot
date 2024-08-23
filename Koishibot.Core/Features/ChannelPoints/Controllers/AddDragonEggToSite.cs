using Koishibot.Core.Features.ChannelPoints.Extensions;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Wordpress;
using Koishibot.Core.Services.Wordpress.Requests;
using Microsoft.AspNetCore.Authorization;
namespace Koishibot.Core.Features.ChannelPoints.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/dragon-quest")]
public class AddDragonEggToSiteController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Dragon Quest"])]
	[AllowAnonymous]
	[HttpPost("wordpress")]
	public async Task<ActionResult> AddDragonEggToSite
		([FromBody] AddDragonEggToSiteCommand e)
	{
		var result = await Mediator.Send(e);
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record AddDragonEggToSiteHandler(
IAppCache Cache,
IOptions<Settings> Settings,
IHttpClientFactory HttpClientFactory,
KoishibotDbContext Database,
IWordpressService WordpressService
) : IRequestHandler<AddDragonEggToSiteCommand, int>
{
	public async Task<int> Handle(AddDragonEggToSiteCommand command, CancellationToken cancel)
	{
		var dragonQuest = Cache.GetDragonEggQuest();
		if (dragonQuest is null) throw new Exception("Dragon Quest not found in cache");
		if (dragonQuest.SuccessfulUser is null) throw new Exception("Winning User not found");

		var databaseEntry = await Database.GetItemTagByUserId(dragonQuest.SuccessfulUser.Id);
		if (databaseEntry is null)
		{
			var result = await WordpressService.CreateItemTag(dragonQuest.SuccessfulUser.Name);
			databaseEntry = new ItemTag { UserId = dragonQuest.SuccessfulUser.Id, WordPressId = result.Id };

			Database.ItemTags.Add(databaseEntry);
			await Database.SaveChangesAsync(cancel);
		}

		var htmlCode = $"<a href=\"https://dragcave.net/view/{command.Code}\"><img src=\"https://dragcave.net/image/{command.Code}.gif\" style=\"border-width:0\" alt=\"Adopt one today!\"/></a>";
		var parameters = new AddItemRequest
		{
			Status = "publish",
			Title = "?",
			Content = htmlCode,
			ItemCategory = 38,
			ItemTag = databaseEntry.WordPressId
		};

		var itemResult = await WordpressService.CreateItem(parameters);

		var dragon = new KoiKinDragon
		{
			WordpressId = itemResult.Id,
			Timestamp = itemResult.Date,
			Code = command.Code,
			Name = "?",
			ItemTagId = databaseEntry.Id
		};

		Database.Add(dragon);
		await Database.SaveChangesAsync(cancel);

		// post in chat link
		// add to overlay

		return dragon.Id;
	}
}


/*═══════════════════【 COMMAND 】═══════════════════*/
public record AddDragonEggToSiteCommand(
string Code
) : IRequest<int>;