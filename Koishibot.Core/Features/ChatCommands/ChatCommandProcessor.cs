using Koishibot.Core.Features.ChatCommands.Interface;
using Koishibot.Core.Features.ChatMessages.Models;
namespace Koishibot.Core.Features.ChatCommands;

// == ⚫ == //

public record ChatCommandProcessor(
	IOptions<Settings> Settings,
	IGeneralCommands GeneralCommands,
	IAttendanceCommands AttendanceCommands,
	IRaidCommands RaidCommands,
	IDandleCommands DandleCommands
	) : IChatCommandProcessor
{
	public bool Processed { get; set; }

	public async Task Start(ChatMessageDto c)
	{
		if (Settings.Value.DebugMode is true)
		{
			Processed = await DandleCommands.Process(c);
			if (Processed is true) { return; }

			Processed = await GeneralCommands.Process(c);
			if (Processed is true) { return; }
		}
		else
		{
			Processed = await GeneralCommands.Process(c);
			if (Processed is true) { return; }

			Processed = await AttendanceCommands.Process(c);
			if (Processed is true) { return; }

			Processed = await DandleCommands.Process(c);
			if (Processed is true) { return; }

			if (c.CommandIsSuggestion())
			{
				await RaidCommands.Process(c);
			}
		}
	}
}