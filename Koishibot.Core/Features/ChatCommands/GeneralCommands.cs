using Koishibot.Core.Features.ChatCommands.Interface;
using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Features.Polls.Enums;
using Koishibot.Core.Features.Polls.Interfaces;
using Koishibot.Core.Features.Todoist.Enums;
using Koishibot.Core.Features.Todoist.Interface;
using Koishibot.Core.Features.Todoist.Models;
namespace Koishibot.Core.Features.ChatCommands;

public record GeneralCommands(
	IPresetPollService PresetPollService,
	ITodoistService TodoistService
	) : IGeneralCommands
{
	public async Task<bool> Process(ChatMessageDto cc)
	{
		switch (cc.Command)
		{
			case "joke":
				await PresetPollService.Start(PollType.DadJoke, cc.User.Name);
				return true;

			case "codetime":
				return true;

			case "later":
				await TodoistService.CreateTask
					(new TodoistTaskDto(cc.User.Name, cc.Message, TaskType.Later));
				return true;

			case "bug":
				await TodoistService.CreateTask
					(new TodoistTaskDto(cc.User.Name, cc.Message, TaskType.Bug));
				return true;

			case "idea":
				await TodoistService.CreateTask
					(new TodoistTaskDto(cc.User.Name, cc.Message, TaskType.Idea));
				return true;

			default:
				return false;
		}
	}
}