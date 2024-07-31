using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatCommands.Enums;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.Todoist.Enums;
using Koishibot.Core.Features.Todoist.Interface;
using Koishibot.Core.Features.Todoist.Models;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
using Todoist.Net;
using Todoist.Net.Models;
namespace Koishibot.Core.Features.Todoist;

// == ⚫ SERVICE == //

public record TodoistService(
	ITodoistClient TodoistClient, 
	ITwitchIrcService BotIrc,
	IChatReplyService ChatReply
	) : ITodoistService
{
	public async Task CreateTask(TodoistTaskDto todoistTask)
	{
		var (label, command)  = todoistTask.Type switch
		{
			TaskType.Later => ("\U0001f7e3 @Twitch", Command.TodoistLater),
			TaskType.Bug => ("\U0001f41b @TwitchBot", Command.TodoistBug),
			TaskType.Idea => ("\u2B50 @TwitchBot", Command.TodoistIdea),
			_ => ("\U0001f7e3 @Twitch", Command.TodoistLater)
		};

		var taskMessage =
			$"{label} {todoistTask.Username}: {todoistTask.Message} Today";

		var task = new QuickAddItem(taskMessage);
		await TodoistClient.Items.QuickAddAsync(task);

		// Todo: Publish a message that task has been added

		await ChatReply.Start(command, new UserData(todoistTask.Username), PermissionLevel.Everyone);
	}
}

// == ⚫ CHAT REPLY == //

public static class TodoistChatReply
{
	public static async Task PostTodoistReply
		(this ITwitchIrcService irc, TaskType type, string username)
	{
		var message = type switch
		{
			TaskType.Later => $"{username}, thanks for the reminder!",
			TaskType.Bug => $"{username}, thanks for the bug report! SQUASH THE BUG!",
			TaskType.Idea => $"{username}, thanks for the stream suggestion!",
			_ => $"Something bork on PostTodoistReply lol"
		};

		await irc.BotSend(message);
	}
}