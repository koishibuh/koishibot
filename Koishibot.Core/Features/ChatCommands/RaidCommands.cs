
using Koishibot.Core.Features.ChatCommands.Interface;
using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.RaidSuggestions.Enums;
using Koishibot.Core.Features.RaidSuggestions.Extensions;
using Koishibot.Core.Features.RaidSuggestions.Interfaces;
namespace Koishibot.Core.Features.ChatCommands;

public record RaidCommands(
	IAppCache Cache,
	IChatReplyService ChatReplyService,
	IRaidSuggestionValidation StreamerValidation
	) : IRaidCommands
{
	public async Task Process(ChatMessageDto cc)
	{
		var user = new UsernameData(cc.User.Name);


		if (Cache.RaidSuggestionDisabled())
		{
			await ChatReplyService.App(Command.StreamNotOver);
		}
		else if (Cache.UpdateStatusToVoting())
		{
			await ChatReplyService.App(Command.RaidSuggestionsClosed, user);
		}
		else if (cc.SuggestionCommandEmpty())
		{
			await ChatReplyService.App(Command.NoSuggestionMade, user);
		}
		else
		{
			var suggestion = cc.GetSuggestedStreamerFromMessage();
			await StreamerValidation.Start(cc.User, suggestion);
		}
	}
}