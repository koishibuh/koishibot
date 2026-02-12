using Koishibot.Core.Features.ChatCommands.Interface;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Features.Dandle;
using Koishibot.Core.Features.Dandle.Enums;
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
		if (c.Command == Command.Definition)
		{
			await DandleWordService.DefineWord(c.Message);
		}

		if (c.Command == Command.FindWord)
		{
			await DandleWordService.FindWord(c);
		}

		// if dandle is enabled and suggestions
		if (Cache.DandleIsClosed())
		{
			if (c.User.Permissions is PermissionLevel.Mod or PermissionLevel.Broadcaster)
			{
				switch (c.Command)
				{
					case Command.AddWord:
						await DandleWordService.CreateWord(c);
						return true;

					case Command.RemoveWord:
						await DandleWordService.DeleteWord(c);
						return true;
				}
			}
			
			else
				return false;
		}

		var dandleInfo = Cache.GetDandleInfo();

		if (Cache.DandleAcceptingSuggestions())
		{
			switch (c.Command)
			{
				case Command.Guess or Command.GuessShort:
					c.Message = c.Message.ToLower();
					await DandleSuggestionProcessor.Start(c);
					return true;

				case Command.Vote or Command.VoteShort:
					// post in chat votes are closed?
					return true;

				default:
					return false;
			}
		}

		if (Cache.DandleAcceptingVotes())
		{
			if (c.Command is not (Command.Vote or Command.VoteShort)) return false;
			await DandleVoteProcessor.ProcessVote(c);
			return true;

		}

		// Dandle processing
		return false;
	}
}