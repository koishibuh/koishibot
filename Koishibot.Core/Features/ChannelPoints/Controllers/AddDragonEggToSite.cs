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

		var htmlCode = command.CreateHtmlCode();
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

		// TODO: post in chat link
		// TODO: add to overlay

		return dragon.Id;
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record AddDragonEggToSiteCommand(
string Code
) : IRequest<int>
{
	public async Task<bool> IsDragonUnique(KoishibotDbContext database)
	{
		var result = await database.KoiKinDragons
		.FirstOrDefaultAsync(x => x.Code == Code);

		return result is null;
	}

	public string CreateHtmlCode()
		=> $"<a href=\"https://dragcave.net/view/{Code}\"><img src=\"https://dragcave.net/image/{Code}.gif\"/></a>";
};

/*══════════════════【 VALIDATOR 】══════════════════*/
public class AddDragonEggToSiteValidator
: AbstractValidator<AddDragonEggToSiteCommand>
{
	public KoishibotDbContext Database { get; }

	public AddDragonEggToSiteValidator(KoishibotDbContext context)
	{
		Database = context;

		RuleFor(p => p.Code)
		.NotEmpty()
		.Length(5);

		RuleFor(p => p)
		.MustAsync(DragonUnique)
		.WithMessage("Dragon Code already exists");
	}

	private async Task<bool> DragonUnique
	(AddDragonEggToSiteCommand command, CancellationToken cancel)
		=> await command.IsDragonUnique(Database);
}