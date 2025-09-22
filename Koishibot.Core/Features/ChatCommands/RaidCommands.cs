
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
		var data = new { User = cc.User.Name };

		if (Cache.RaidSuggestionDisabled())
		{
			await ChatReplyService.CreateResponse(Response.StreamNotOver);
		}
		else if (Cache.UpdateStatusToVoting())
		{
			await ChatReplyService.CreateResponse(Response.RaidSuggestionsClosed, data);
		}
		else if (cc.SuggestionCommandEmpty())
		{
			await ChatReplyService.CreateResponse(Response.NoSuggestionMade, data);
		}
		else
		{
			var suggestion = cc.GetSuggestedStreamerFromMessage();
			await StreamerValidation.Start(cc.User, suggestion);
		}
	}
}