using HandlebarsDotNet;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
namespace Koishibot.Core.Features.ChatCommands;

public record ChatReplyService(
	IAppCache Cache,
	ITwitchIrcService TwitchIrc,
	KoishibotDbContext Database) : IChatReplyService
{
	public async Task Start<T>(string command, T data, PermissionLevel permission)
	{
		var result = Cache.GetCommand(command, permission);

		if (result is null)
		{
			var databaseResult = await Database.GetCommand(command)
				?? throw new Exception("Command not found");

			Cache.AddCommand(databaseResult);

			var successful = databaseResult.TryGetValue(command, out result);
			if (successful is false) throw new Exception("Command not found");
		}

		var template = Handlebars.Compile(result.Message);
		var generatedText = template(data);

		await TwitchIrc.BotSend(generatedText);
	}

	// APPLICATION

	public async Task App(string command)
	{
		var result = Cache.GetCommand(command, PermissionLevel.App);

		if (result is null)
		{
			var databaseResult = await Database.GetCommand(command)
				?? throw new Exception("Command not found");

			Cache.AddCommand(databaseResult);

			var successful = databaseResult.TryGetValue(command, out result);
			if (successful is false) throw new Exception("Command not found");
		}

		await TwitchIrc.BotSend(result.Message);
	}

	public async Task App<T>(string command, T data)
	{
		var result = Cache.GetCommand(command, PermissionLevel.App);

		if (result is null)
		{
			var databaseResult = await Database.GetCommand(command)
				?? throw new Exception("Command not found");

			Cache.AddCommand(databaseResult);

			var successful = databaseResult.TryGetValue(command, out result);
			if (successful is false) throw new Exception("Command not found");
		}

		var template = Handlebars.Compile(result.Message);
		var generatedText = template(data);

		await TwitchIrc.BotSend(generatedText);
	}
}

public interface IChatReplyService
{
	Task Start<T>(string command, T data, PermissionLevel permission);
	Task App(string command);
	Task App<T>(string command, T data);
}