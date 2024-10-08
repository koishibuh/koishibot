using Koishibot.Core.Features.ChatCommands.Interface;
using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Features.Dandle;
using Koishibot.Core.Features.Dandle.Extensions;
using Koishibot.Core.Features.Dandle.Interfaces;

namespace Koishibot.Core.Features.ChatCommands;

public record DandleCommands(
IAppCache Cache,
IDandleSuggestionProcessor DandleSuggestionProcessor,
IDandleVoteProcessor DandleVoteProcessor,
IDandleWordService DandleWordService
) : IDandleCommands
{
	public async Task<bool> Process(ChatMessageDto c)
	{
		if (c.Command == "define")
		{
			await DandleWordService.DefineWord(c.Message);
		}

		// if dandle is enabled and suggestions
		if (Cache.DandleIsClosed())
		{
			if (c.User.Login is "spacey3d" or "elysiagriffin")
			{
				switch (c.Command)
				{
					case "addword":
						await DandleWordService.CreateWord(c);
						return true;

					case "removeword":
						await DandleWordService.DeleteWord(c);
						return true;
				}
			}
			return false;
		}

		var dandleInfo = Cache.GetDandleInfo();

		if (Cache.DandleAcceptingSuggestions())
		{
			switch (c.Command)
			{
				case "guess" or "g":
					c.Message = c.Message.ToLower();
					await DandleSuggestionProcessor.Start(c);
					return true;

				case "vote" or "v":
					// post in chat votes are closed?
					return true;

				default:
					return false;
			}
		}

		if (Cache.DandleAcceptingVotes())
		{
			if (c.Command is not ("vote" or "v")) return false;
			await DandleVoteProcessor.ProcessVote(c);
			return true;

		}

		// Dandle processing
		return false;
	}
}