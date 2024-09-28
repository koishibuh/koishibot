using Koishibot.Core.Features.ChannelPoints.Enums;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.Wordpress;
using Koishibot.Core.Services.Wordpress.Requests;

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
IWordpressService WordpressService,
IChatReplyService ChatReplyService
) : IRequestHandler<AddDragonEggToSiteCommand, int>
{
	public async Task<int> Handle(AddDragonEggToSiteCommand command, CancellationToken cancel)
	{
		var dragonQuestWinner = Cache.GetDragonQuestWinner();
		if (dragonQuestWinner is null) throw new Exception("Winning User not found");

		var databaseEntry = await Database.GetItemTagByUserId(dragonQuestWinner.Id)
			?? await CreateUserItemTag(dragonQuestWinner);

		var parameters = command.CreateWordpressItemTag(databaseEntry.Id);
		var itemResult = await WordpressService.CreateItem(parameters);

		var dragon = new KoiKinDragon
		{
			WordpressId = itemResult.Id,
			Timestamp = itemResult.Date,
			Code = command.Code,
			Name = "?",
			ItemTagId = databaseEntry.Id
		};

		await Database.UpdateEntry(dragon);

		var template = command.CreateTemplate();
		await ChatReplyService.App(Command.DragonQuestNewestEgg, template);

		Cache.Remove(CacheName.DragonQuest);
		await Cache.UpdateServiceStatusOffline(ServiceName.DragonQuest);

		// TODO: add to overlay
		return dragon.Id;
	}

	/*═══════════════════【】═══════════════════*/
	private async Task<WordpressItemTag> CreateUserItemTag(TwitchUser dragonQuestWinner)
	{
		var result = await WordpressService.CreateItemTag(dragonQuestWinner.Name);
		var databaseEntry = new WordpressItemTag(dragonQuestWinner.Id, result.Id);

		Database.WordpressItemTags.Add(databaseEntry);
		await Database.SaveChangesAsync();
		return databaseEntry;
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

	public AddItemRequest CreateWordpressItemTag(int id) => new()
	{
		Status = "publish",
		Title = "?",
		Content = CreateHtmlCode(),
		ItemCategory = 38,
		ItemTag = id
	};

	private string CreateHtmlCode() =>
		$"<a href=\"https://dragcave.net/view/{Code}\"><img src=\"https://dragcave.net/image/{Code}.gif\"/></a>";

	public object CreateTemplate() => new { url = $"https://dragcave.net/view/{Code}" };
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