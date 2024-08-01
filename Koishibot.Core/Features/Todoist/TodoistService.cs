using HandlebarsDotNet;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatCommands.Enums;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.Todoist.Interface;
using Koishibot.Core.Persistence;
using Todoist.Net;
using Todoist.Net.Models;
namespace Koishibot.Core.Features.Todoist;

// == ⚫ SERVICE == //

public record TodoistService(
	IAppCache Cache,
	ITodoistClient TodoistClient,
	KoishibotDbContext Database,
	IChatReplyService ChatReply
	) : ITodoistService
{
	public async Task CreateTask(string command, string username, string taskMessage)
	{
		var todoistMessage = await GetTaskMessageFromStorage(command);

		var template = Handlebars.Compile(todoistMessage);
		var generatedText = template(new UserMessageData(username, taskMessage));

		var task = new QuickAddItem(generatedText);
		await TodoistClient.Items.QuickAddAsync(task);

		// Todo: Publish a message that task has been added

		await ChatReply.Start(command, new UserData(username), PermissionLevel.Everyone);
	}

	// == ⚫ == //

	public async Task<string> GetTaskMessageFromStorage(string command)
	{
		var todoistMessage = command switch
		{
			Command.Idea => "task-idea",
			Command.Bug => "task-bug",
			Command.Later => "task-later",
			_ => "task-later"
		};

		var result = Cache.GetCommand(todoistMessage, PermissionLevel.Everyone);

		if (result is null)
		{
			var databaseResult = await Database.GetCommand(todoistMessage)
				?? throw new Exception("Command not found");

			Cache.AddCommand(databaseResult);

			var successful = databaseResult.TryGetValue(todoistMessage, out result);
			if (successful is false) throw new Exception("Command not found");
		}

		return result.Message;
	}
}