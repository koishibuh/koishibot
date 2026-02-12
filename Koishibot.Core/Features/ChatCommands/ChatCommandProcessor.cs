using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatCommands.Interface;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Services.Twitch.Irc;
namespace Koishibot.Core.Features.ChatCommands;

/*═══════════════════【 SERVICE 】═══════════════════*/
public record ChatCommandProcessor(
IAppCache Cache,
IOptions<Settings> Settings,
IGeneralCommands GeneralCommands,
IAttendanceCommands AttendanceCommands,
IRaidCommands RaidCommands,
IDandleCommands DandleCommands,
ITwitchIrcService TwitchIrcService
) : IChatCommandProcessor
{
	private bool Processed { get; set; }

// TODO: Check global/user timers?

	public async Task Start(ChatMessageDto c)
	{
		if (Settings.Value.DebugMode is true)
		{
			// if (c.CommandIsSuggestion()) 
			// {
			// 	await RaidCommands.Process(c);
			// }
			// await DandleCommands.Process(c);
			
			await DandleCommands.Process(c);
		}
		else
		{
			// Reminder for future: need a check for mod vs everyone
			var result = Cache.NewGetCommand(c.Command);
			if (result is null) return; // Command not found Todo: Post message?
			
			switch (result.Category)
			{
				case CommandCategory.Static:
					var response = GetResponse(result);
					await TwitchIrcService.BotSend(response.Message);
					break;
				case CommandCategory.General:
					await GeneralCommands.Process(c);
					break;
				case CommandCategory.Attendance:
					await AttendanceCommands.Process(c);
					break;
				case CommandCategory.Dandle:
					await DandleCommands.Process(c);
					break;
			}

			//Todo: If there is not a function for a valid Category 
		}
	}

	private ChatResponseDto GetResponse(NewChatCommandDto dto)
	{
		// find response using the id from command
		return dto.ResponseId is null 
			? throw new Exception($"ResponseId for command {dto.Name} is null") 
			: Cache.GetResponseById(dto.ResponseId);
	}
}