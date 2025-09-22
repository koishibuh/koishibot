using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Features.Dandle.Enums;
using Koishibot.Core.Features.Dandle.Extensions;
using Koishibot.Core.Features.Dandle.Models;
using Koishibot.Core.Persistence;
using Newtonsoft.Json;

namespace Koishibot.Core.Features.Dandle;

public record DandleWordService(
IServiceScopeFactory ServiceScopeFactory,
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
			await ChatReplyService.CreateResponse(Response.InvalidWordLength);
			return;
		}

		if (c.MessageContainsNonLetters())
		{
			await ChatReplyService.CreateResponse(Response.InvalidWord);
			return;
		}

		using var scope = ServiceScopeFactory.CreateScope();
		var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();

		var result = await database.FindDandleWord(c.Message);
		if (result is not null)
		{
			var data = new { Word = c.Message };
			await ChatReplyService.CreateResponse(Response.WordAddedFailed, data);
		}
		else
		{
			var dandleWord = new DandleWord().Set(c.Message);
			await database.UpdateEntry(dandleWord);

			var data = new { Word = c.Message };
			await ChatReplyService.CreateResponse(Response.WordAdded, data);
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
			await ChatReplyService.CreateResponse(Response.WordRemoved, data);
		}
		else
		{
			var data = new { Word = c.Message };
			await ChatReplyService.CreateResponse(Response.WordRemovedFailed, data);
		}
	}

	/*═════════◢◣═════════*/
	public async Task FindWord(ChatMessageDto c)
	{
		using var scope = ServiceScopeFactory.CreateScope();
		var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();

		var word = await database.FindDandleWord(c.Message);
		if (word is not null)
		{
			var data = new { Word = word.Word };
			await ChatReplyService.CreateResponse(Response.FindWord, data);
		}
		else
		{
			var data = new { Word = c.Message };
			await ChatReplyService.CreateResponse(Response.WordNotFound, data);
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
				await ChatReplyService.CreateResponse(Response.Definition, data);
			}
		}
		else
		{
			var data = new { Word = word };
			await ChatReplyService.CreateResponse(Response.NoDefinition, data);
		}
	}
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IDandleWordService
{
	Task CreateWord(ChatMessageDto c);
	Task DeleteWord(ChatMessageDto c);
	Task FindWord(ChatMessageDto c);
	Task DefineWord(string word);
}