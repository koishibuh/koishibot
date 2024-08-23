using Koishibot.Core.Features.ChannelPoints.Enums;
using Koishibot.Core.Features.ChannelPoints.Extensions;
using Koishibot.Core.Features.ChannelPoints.Interfaces;
using Koishibot.Core.Features.ChatMessages.Models;
namespace Koishibot.Core.Features.ChatCommands;

/*═══════════════════【 SERVICE 】═══════════════════*/
public record DragonQuestCommands(
IAppCache Cache,
IDragonEggQuestService DragonEggQuestService,
IChatReplyService BotIrc
) : IDragonQuestCommands
{
	public async Task<bool> Process(ChatMessageDto c)
	{
		// check winning user
		var winner = Cache.GetDragonQuestWinner();
		if (winner is null || winner.Name != c.User.Name) return false;

		var url = c.Command switch
		{
		"coast" => "https://dragcave.net/locations/1-coast",
		"desert" => "https://dragcave.net/locations/2-desert",
		"forest" => "https://dragcave.net/locations/3-forest",
		"jungle" => "https://dragcave.net/locations/4-jungle",
		"alpine" => "https://dragcave.net/locations/5-alpine",
		"volcano" => "https://dragcave.net/locations/6-volcano",
		_ => ""
		};

		if (string.IsNullOrEmpty(url)) { return false; }

		var eggDescriptions = await DragonEggQuestService.GetEggDescriptions(url);

		var template = new
		{
			User = c.User.Name,
			Choice1 = eggDescriptions[0] ?? string.Empty,
			Choice2 = eggDescriptions[1] ?? string.Empty,
			Choice3 = eggDescriptions[2] ?? string.Empty
		};

		await BotIrc.App(Command.DragonEggQuestPickEgg, template);
		// TODO: display descriptions on screen
		return true;
	}
}

/*═══════════════════【 INTERFACE 】═══════════════════*/
public interface IDragonQuestCommands
{
	Task<bool> Process(ChatMessageDto c);
}