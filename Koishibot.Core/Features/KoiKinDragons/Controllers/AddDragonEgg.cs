using Koishibot.Core.Features.ChannelPoints.Enums;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.KoiKinDragons.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;
namespace Koishibot.Core.Features.KoiKinDragons.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/dragons")]
public class AddDragonEggController : ApiControllerBase
{
	[HttpPost]
	public async Task<ActionResult> AddDragonEgg
		([FromBody] AddDragonEggCommand e)
	{
		var result = await Mediator.Send(e);
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record AddDragonEggToSiteHandler(
IAppCache Cache,
KoishibotDbContext Database,
IChatReplyService ChatReplyService
) : IRequestHandler<AddDragonEggCommand, int>
{
	public async Task<int> Handle(AddDragonEggCommand command, CancellationToken cancel)
	{
		var dragonQuestWinner = Cache.GetDragonQuestWinner();
		if (dragonQuestWinner is null) throw new Exception("Winning User not found");
		
		var dragon = await SaveDragonToDatabase(command, dragonQuestWinner.Id);

		var template = command.CreateTemplate();
		await ChatReplyService.App(Command.DragonQuestNewestEgg, template);

		await Cache
			.RemoveDragonQuest()
			.UpdateServiceStatusOffline(ServiceName.DragonQuest);

		// TODO: add to overlay
		return dragon;
	}

	/*═══════════════════【】═══════════════════*/
	private async Task<int> SaveDragonToDatabase(AddDragonEggCommand command, int userId)
	{
		var dragon = new KoiKinDragon
		{
			Timestamp = DateTime.UtcNow,
			Code = command.Code,
			Name = "?",
			UserId = userId
		};

		return await Database.UpdateEntry(dragon);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record AddDragonEggCommand(
string Code
) : IRequest<int>
{
	public async Task<bool> IsDragonUnique(KoishibotDbContext database)
	{
		var result = await database.KoiKinDragons
			.FirstOrDefaultAsync(x => x.Code == Code);

		return result is null;
	}

	// public AddItemRequest CreateWordpressItem(int id) => new()
	// {
	// 	Status = "publish",
	// 	Title = "?",
	// 	Content = CreateHtmlCode(),
	// 	ItemCategory = 38,
	// 	ItemTag = id
	// };

	// private string CreateHtmlCode() =>
	// 	$"<a href=\"https://dragcave.net/view/{Code}\"><img src=\"https://dragcave.net/image/{Code}.gif\"/></a>";
	//
	public object CreateTemplate() => new { url = $"https://dragcave.net/view/{Code}" };
};

/*══════════════════【 VALIDATOR 】══════════════════*/
public class AddDragonEggToSiteValidator
	: AbstractValidator<AddDragonEggCommand>
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
		(AddDragonEggCommand command, CancellationToken cancel)
		=> await command.IsDragonUnique(Database);
}