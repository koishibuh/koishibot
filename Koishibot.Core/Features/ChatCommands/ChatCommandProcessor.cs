using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.ChatCommands.Interface;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Services.Twitch.Irc;
using System.ComponentModel;
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

	public async Task Start(ChatMessageDto c)
	{
		if (Settings.Value.DebugMode is true)
		{
			if (c.CommandIsSuggestion())
			{
				await RaidCommands.Process(c);
			}
		}
		else
		{
			var result = Cache.GetCommand(c.Command, PermissionLevel.Everyone);
			if (result is null) return; // Command not found Todo: Post message?
			
			switch (result.Category)
			{
				case CommandCategory.Static:
					await TwitchIrcService.BotSend(result.Message);
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
}