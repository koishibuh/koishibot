using Koishibot.Core.Features.Todoist.Enums;
using Koishibot.Core.Features.Todoist.Interface;
using Koishibot.Core.Features.Todoist.Models;
using Todoist.Net;
using Todoist.Net.Models;
namespace Koishibot.Core.Features.Todoist;

// == ⚫ SERVICE == //

public record TodoistService(
	ITodoistClient TodoistClient, IChatMessageService BotIrc
	)	: ITodoistService
{
	public async Task CreateTask(TodoistTaskDto todoistTask)
	{
		var label = todoistTask.Type switch
		{
			TaskType.Reminder => "\U0001f7e3 @Twitch",
			TaskType.Bug			=> "\U0001f41b @TwitchBot",
			TaskType.Idea			=> "\u2B50 @TwitchBot",
			_									=> "\U0001f7e3 @Twitch"
		};

		var taskMessage =
			$"{label} {todoistTask.Username}: {todoistTask.Message} Today";

		var task = new QuickAddItem(taskMessage);
		await TodoistClient.Items.QuickAddAsync(task);

		// Todo: Publish a message that task has been added
		await BotIrc.PostTodoistReply(todoistTask.Type, todoistTask.Username);
	}
}

// == ⚫ CHAT REPLY == //

public static class TodoistChatReply
{
	public static async Task PostTodoistReply
		(this IChatMessageService irc, TaskType type, string username)
	{
		var message = type switch
		{
			TaskType.Reminder => $"{username}, thanks for the reminder!",
			TaskType.Bug			=> $"{username}, thanks for the bug report! SQUASH THE BUG!",
			TaskType.Idea			=> $"{username}, thanks for the stream suggestion!",
			_ => $"Something bork on PostTodoistReply lol"
		};

		await irc.BotSend(message);
	}
}
