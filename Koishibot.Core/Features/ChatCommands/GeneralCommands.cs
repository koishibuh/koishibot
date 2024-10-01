using Koishibot.Core.Features.ChatCommands.Enums;
using Koishibot.Core.Features.ChatCommands.Interface;
using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Features.Polls;
using Koishibot.Core.Features.Polls.Enums;
using Koishibot.Core.Features.Todoist.Interface;
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

			case Command.Later:
				await TodoistService.CreateTask(Command.Later, cc.User.Name, cc.Message);
				return true;

			case Command.Bug:
				await TodoistService.CreateTask(Command.Bug, cc.User.Name, cc.Message);
				return true;

			case Command.Idea:
				await TodoistService.CreateTask(Command.Idea, cc.User.Name, cc.Message);
				return true;

			default:
				return false;
		}
	}
}