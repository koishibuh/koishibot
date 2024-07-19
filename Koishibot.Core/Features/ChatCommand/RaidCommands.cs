using Koishibot.Core.Features.ChatCommand.Interface;
using Koishibot.Core.Features.ChatCommand.Models;
using Koishibot.Core.Features.Raids.Enums;
using Koishibot.Core.Features.RaidSuggestions;
using Koishibot.Core.Features.RaidSuggestions.Extensions;
using Koishibot.Core.Features.RaidSuggestions.Interfaces;
namespace Koishibot.Core.Features.ChatCommand;

public record RaidCommands(IAppCache Cache,
	IChatMessageService BotIrc, IRaidSuggestionValidation StreamerValidation
	) : IRaidCommands
{
	public async Task Process(ChatMessageCommand cc)
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