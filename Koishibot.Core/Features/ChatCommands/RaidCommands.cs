using Koishibot.Core.Features.ChatCommands.Interface;
using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Features.Raids.Enums;
using Koishibot.Core.Features.RaidSuggestions;
using Koishibot.Core.Features.RaidSuggestions.Extensions;
using Koishibot.Core.Features.RaidSuggestions.Interfaces;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
namespace Koishibot.Core.Features.ChatCommands;

public record RaidCommands(IAppCache Cache,
	ITwitchIrcService BotIrc,
	IRaidSuggestionValidation StreamerValidation
	) : IRaidCommands
{
	public async Task Process(ChatMessageDto cc)
	{
		if (Cache.RaidSuggestionDisabled())
		{
			await BotIrc.RaidStatus
				(Code.NotCurrentlyRaiding);
		}
		else if (Cache.UpdateStatusToVoting())
		{
			await BotIrc.PostSuggestionResult
				(Code.RaidSuggestionsClosed, (cc.User.Name, ""));
		}
		else if (cc.SuggestionCommandEmpty())
		{
			await BotIrc.PostSuggestionResult
				(Code.NoSuggestionMade, (cc.User.Name, ""));
		}
		else
		{
			var suggestion = cc.GetSuggestedStreamerFromMessage();
			await StreamerValidation.Start(cc.User, suggestion);
		}
	}
}