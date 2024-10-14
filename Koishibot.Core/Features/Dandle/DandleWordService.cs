using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatCommands.Extensions;
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
IServiceScopeFactory ServiceScopeFactory,
// KoishibotDbContext Database,
IChatReplyService ChatReplyService,
IHttpClientFactory HttpClientFactory
) : IDandleWordService
{
	// todo: check if word is actual word

	/*═════════◢◣═════════*/
	public async Task CreateWord(ChatMessageDto c)
	{
		if (c.MessageCorrectLength(5) is false)
		{
			await ChatReplyService.App(Command.InvalidLength);
			return;
		}

		if (c.MessageContainsNonLetters())
		{
			await ChatReplyService.App(Command.InvalidWord);
			return;
		}

		using var scope = ServiceScopeFactory.CreateScope();
		var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();

		var result = await database.FindDandleWord(c.Message);
		if (result is not null)
		{
			var data = new { Word = c.Message };
			await ChatReplyService.App(Command.WordExists, data);
		}
		else
		{
			var dandleWord = new DandleWord().Set(c.Message);
			await database.UpdateEntry(dandleWord);

			var data = new { Word = c.Message };
			await ChatReplyService.App(Command.WordAdded, data);
		}
	}

	/*═════════◢◣═════════*/
	public async Task DeleteWord(ChatMessageDto c)
	{
		using var scope = ServiceScopeFactory.CreateScope();
		var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();

		var word = await database.FindDandleWord(c.Message);
		if (word is not null)
		{
			await database.RemoveDandleWord(word);
			var data = new { Word = word.Word };
			await ChatReplyService.App(Command.WordRemoved, data);
		}
		else
		{
			var data = new { Word = c.Message };
			await ChatReplyService.App(Command.WordNotFound, data);
		}
	}

	/*═════════◢◣═════════*/
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