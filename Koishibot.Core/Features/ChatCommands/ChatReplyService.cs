using HandlebarsDotNet;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.Irc;
namespace Koishibot.Core.Features.ChatCommands;

/*═══════════════════【 SERVICE 】═══════════════════*/
public record ChatReplyService(
IAppCache Cache,
ITwitchIrcService TwitchIrc,
IServiceScopeFactory ScopeFactory
) : IChatReplyService
{
	public async Task CreateResponse(string command)
	{
		var result = await GetResponse(command);
		await TwitchIrc.BotSend(result);
	}

	public async Task CreateResponse<T>(string command, T data)
	{
		var result = await GetResponse(command);
		var template = Handlebars.Compile(result);
		var generatedText = template(data);

		await TwitchIrc.BotSend(generatedText);
	}
	
	private async Task<string> GetResponse(string command)
	{
		var result = Cache.GetResponse(command);

		if (result is not null)
			return result.Message;

		using var scope = ScopeFactory.CreateScope();
		var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();
		result = await database.GetResponse(command);
		if (result is null)
			throw new Exception($"Command '{command}' not found.");

		Cache.AddResponse(result);

		return result.Message;
	}
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IChatReplyService
{
	Task CreateResponse(string command);
	Task CreateResponse<T>(string command, T data);
}