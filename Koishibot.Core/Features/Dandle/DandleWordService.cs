using Koishibot.Core.Features.ChatCommand.Models;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Dandle.Extensions;
using Koishibot.Core.Features.Dandle.Interfaces;
using Koishibot.Core.Features.Dandle.Models;
using Koishibot.Core.Persistence;
using Newtonsoft.Json;
namespace Koishibot.Core.Features.Dandle;

public record DandleWordService(
	KoishibotDbContext Database,
	IChatMessageService BotIrc,
	IHttpClientFactory HttpClientFactory
	) : IDandleWordService
{
	// todo: check if word is actual word
	public async Task CreateWord(ChatMessageCommand c)
	{
		if (c.Message.Length != 5)
		{
			await BotIrc.DandleWordLengthIncorrect();
			return;
		}

		if (Toolbox.StringContainsNonLetters(c.Message))
		{
			await BotIrc.DandleWordNotValid();
			return;
		}

		var result = await Database.DandleWords
			.FirstOrDefaultAsync(p => p.Word == c.Message);

		if (result is not null)
		{
			await BotIrc.DandleWordExists(c.Message);
		}
		else
		{
			var word = new DandleWord().Set(c.Message);
			await Database.UpdateDandleWord(word);
			await BotIrc.DandleWordCreated(c.Message);
		}
	}

	public async Task DeleteWord(ChatMessageCommand c)
	{
		var word = await Database.FindDandleWord(c.Message);
		if (word is not null)
		{
			await Database.RemoveDandleWord(word);
			await BotIrc.DandleWordDeleted(c.Message);
		} 
		else
		{
			await BotIrc.BotSend($"Unable to remove the word '{c.Message}', not found in Dandle Dictionary");		
		}
	}

	public async Task DefineWord(string word)
	{
		var httpClient = HttpClientFactory.CreateClient("Dictionary");
		var response = await httpClient.GetAsync(word);

		if (response.IsSuccessStatusCode)
		{
			var responseBody = await response.Content.ReadAsStringAsync();

			var definition = JsonConvert.DeserializeObject<List<DandleDefinition>>(responseBody);

			if (definition.Count > 0)
			{
				await BotIrc.BotSend($"{definition[0]?.Word}: {definition[0]?.Meanings[0]?.Definitions[0].Definition}");
			}
		}
		else
		{
			await BotIrc.BotSend($"Definition for {word} not found");
		}
	}
}


public static class DandleWordChatReply
{
	public static async Task DandleWordLengthIncorrect
	(this IChatMessageService botIrc)
	{
		await botIrc.BotSend($"Word needs to be 5 characters long to be added to the Dandle dictionary");
	}

	public static async Task DandleWordExists
		(this IChatMessageService botIrc, string word)
	{
		await botIrc.BotSend($"'{word}' already exists in the Dandle Dictionary");
	}
	public static async Task DandleWordNotValid
	(this IChatMessageService botIrc)
	{
		await botIrc.BotSend($"Dandle words cannot contain numbers or punctuation");
	}

	public static async Task DandleWordCreated
	(this IChatMessageService botIrc, string word)
	{
		await botIrc.BotSend($"Successfully added the word '{word}' to the Dandle Dictionary");
	}

	public static async Task DandleWordDeleted
(this IChatMessageService botIrc, string word)
	{
		await botIrc.BotSend($"Succesfully yeeted the word '{word}' from the Dandle Dictionary");
	}
}