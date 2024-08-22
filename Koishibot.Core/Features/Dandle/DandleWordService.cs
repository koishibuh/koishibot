using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Dandle.Enums;
using Koishibot.Core.Features.Dandle.Extensions;
using Koishibot.Core.Features.Dandle.Interfaces;
using Koishibot.Core.Features.Dandle.Models;
using Koishibot.Core.Persistence;
using Newtonsoft.Json;
namespace Koishibot.Core.Features.Dandle;

public record DandleWordService(
	KoishibotDbContext Database,
	IChatReplyService ChatReplyService,
	IHttpClientFactory HttpClientFactory
	) : IDandleWordService
{
	// todo: check if word is actual word
	public async Task CreateWord(ChatMessageDto c)
	{
		if (c.Message.Length != 5)
		{
			await ChatReplyService.App(Command.InvalidLength);
			return;
		}

		if (Toolbox.StringContainsNonLetters(c.Message))
		{
			await ChatReplyService.App(Command.InvalidWord);
			return;
		}

		var result = await Database.DandleWords
			.FirstOrDefaultAsync(p => p.Word == c.Message);

		if (result is not null)
		{
			var data = new WordData(result.Word);
			await ChatReplyService.App(Command.WordExists, data);
		}
		else
		{
			var dandleWord = new DandleWord().Set(c.Message);
			await Database.UpdateDandleWord(dandleWord);

			var data = new WordData(dandleWord.Word);
			await ChatReplyService.App(Command.WordAdded);
		}
	}

	public async Task DeleteWord(ChatMessageDto c)
	{
		var word = await Database.FindDandleWord(c.Message);
		if (word is not null)
		{
			await Database.RemoveDandleWord(word);
			var data = new WordData(word.Word);
			await ChatReplyService.App(Command.WordRemoved, data);
		}
		else
		{
			var data = new WordData(c.Message);
			await ChatReplyService.App(Command.WordNotFound, data);
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

			if (definition is not null && definition.Count > 0)
			{
				var data = new WordDefineData(definition[0]?.Word, definition[0]?.Meanings[0]?.Definitions[0].Definition);
				await ChatReplyService.App(Command.Definition, data);
			}
		}
		else
		{
			var data = new { Word = word };
			await ChatReplyService.App(Command.NoDefinition, data);
		}
	}
}