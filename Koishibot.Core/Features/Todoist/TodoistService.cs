using HandlebarsDotNet;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatCommands.Enums;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Persistence;
using Todoist.Net;
using Todoist.Net.Models;
namespace Koishibot.Core.Features.Todoist;

/*═══════════════════【 SERVICE 】═══════════════════*/
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

		var data = new  { User = username };
		var responseName = command switch
		{
			Command.Later => Response.Later,
			Command.Idea => Response.Idea,
			Command.Bug => Response.Bug,
			_ => Response.Later
		};
		await ChatReply.CreateResponse(responseName, data);
	}

	// == ⚫ == //

	public async Task<string> GetTaskMessageFromStorage(string command)
	{
		var todoistCommand = command switch
		{
			Command.Idea => "task-idea",
			Command.Bug => "task-bug",
			Command.Later => "task-later",
			_ => "task-later"
		};

		var result = Cache.GetResponse(todoistCommand);

		if (result is null)
		{
			var databaseResult = await Database.GetResponseByName(todoistCommand)
				?? throw new Exception("Command not found");

			Cache.AddResponse(databaseResult);
		}

		return result.Message;
	}
}

/*═══════════════════【 INTERFACE 】═══════════════════*/
public interface ITodoistService
{
	Task CreateTask(string command, string username, string taskMessage);
}