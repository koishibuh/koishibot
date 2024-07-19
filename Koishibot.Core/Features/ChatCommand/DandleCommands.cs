using Koishibot.Core.Features.ChatCommand.Interface;
using Koishibot.Core.Features.ChatCommand.Models;
using Koishibot.Core.Features.Dandle;
using Koishibot.Core.Features.Dandle.Extensions;
using Koishibot.Core.Features.Dandle.Interfaces;

namespace Koishibot.Core.Features.ChatCommand;

public record DandleCommands(
	IAppCache Cache,
	IDandleSuggestionProcessor DandleSuggestionProcessor,
	IDandleVoteProcessor DandleVoteProcessor, 
	IDandleWordService DandleWordService
	) : IDandleCommands
{
	public async Task<bool> Process(ChatMessageCommand c)
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
			if (c.User.Login is "spacey3d" || c.User.Login is "elysiagriffin")
			{
				if (c.Command is "addword")
				{
					// add word
					await DandleWordService.CreateWord(c);
					return true;
				}
				if (c.Command is "removeword")
				{
					// remove
					await DandleWordService.DeleteWord(c);
					return true;
				}
			}
			return false;
		}

		var dandleInfo = Cache.GetDandleInfo();

		if (Cache.DandleAcceptingSuggestions())
		{
			if (c.Command == "guess" || c.Command == "g")
			{
				// !guess
				//if (c.User.Name != "ElysiaGriffin") { return true; }
				c.Message = c.Message.ToLower();
				await DandleSuggestionProcessor.Start(c);
				
				return true;
			}
			else if (c.Command == "vote" || c.Command == "v")
			{
				// post in chat votes are closed?
				return true;
			}
			else
			{
				return false;
			}
		}
		else if (Cache.DandleAcceptingVotes())
		{
			// !vote
			if (c.Command == "vote" || c.Command == "v")
			{
				await DandleVoteProcessor.ProcessVote(c);
				return true;
			}
			return false;
		}
		else
		{
			// Dandle processing
			return false;
		}
	}
}
