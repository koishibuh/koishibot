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
			// add or remove word 
			// if the user is spacey
			if (c.User.Login is "spacey3d" or "elysiagriffin")
			{
				if (c.Command is "addword")
				{
					await DandleWordService.CreateWord(c);
					return true;
				}

				if (c.Command is "removeword")
				{
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
					// !guess
					//if (c.User.Name != "ElysiaGriffin") { return true; }
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
			// !vote
			if (c.Command is not ("vote" or "v")) return false;
			await DandleVoteProcessor.ProcessVote(c);
			return true;

		}

		// Dandle processing
		return false;
	}
}