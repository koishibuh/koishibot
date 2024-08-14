using HandlebarsDotNet;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatCommands.Models;
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
	public async Task Start<T>(string command, T data, string permission)
	{
		var result = Cache.GetCommand(command, permission);
		if (result is null)
		{
			using var scope = ScopeFactory.CreateScope();
			var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();
			var databaseResult = await database.GetCommand(command)
			                     ?? throw new Exception("Command not found");

			Cache.AddCommand(databaseResult);

			var successful = databaseResult.TryGetValue(command, out result);
			if (successful is false) throw new Exception("Command not found");
		}

		var template = Handlebars.Compile(result!.Message);
		var generatedText = template(data);

		await TwitchIrc.BotSend(generatedText);
	}

	public async Task App(string command)
	{
		var result = Cache.GetCommand(command, PermissionLevel.App);
		if (result is null)
		{
			using var scope = ScopeFactory.CreateScope();
			var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();
			var databaseResult = await database.GetCommand(command)
			                     ?? throw new Exception("Command not found");

			Cache.AddCommand(databaseResult);

			var successful = databaseResult.TryGetValue(command, out result);
			if (successful is false) throw new Exception("Command not found");
		}

		await TwitchIrc.BotSend(result!.Message);
	}

	public async Task App<T>(string command, T data)
	{
		var result = Cache.GetCommand(command, PermissionLevel.App);

		if (result is null)
		{
			using var scope = ScopeFactory.CreateScope();
			var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();
			var databaseResult = await database.GetCommand(command)
			                     ?? throw new Exception("Command not found");

			Cache.AddCommand(databaseResult);

			var successful = databaseResult.TryGetValue(command, out result);
			if (successful is false) throw new Exception("Command not found");
		}

		var template = Handlebars.Compile(result!.Message);
		var generatedText = template(data);

		await TwitchIrc.BotSend(generatedText);
	}
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IChatReplyService
{
	Task Start<T>(string command, T data, string permission);
	Task App(string command);
	Task App<T>(string command, T data);
}