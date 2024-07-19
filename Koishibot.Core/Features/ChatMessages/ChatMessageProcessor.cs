using Koishibot.Core.Features.ChatCommand.Interface;
using Koishibot.Core.Features.ChatCommand.Models;
using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Features.TwitchUsers.Interfaces;

namespace Koishibot.Core.Features.ChatMessages;

public record ProcessChatMessageCommand(ChatMessageEvent e) : IRequest;


/// <summary>
/// Processes chat messages: <br/>
/// Publish to UI, Log User, Save Attendance if Enabled, Check Command
/// </summary>
/// <param name="UserId"></param>
/// <param name="Username"></param>
/// <param name="DisplayName"></param>
/// <param name="Badges"></param>
/// <param name="Color"></param>
/// <param name="Message"></param>
public record ChatMessageProcessor(
	IOptions<Settings> Settings,
	IAppCache Cache,
	ISignalrService Signalr, 
	IChatCommandProcessor ChatCommandProcessor,
	ITwitchUserHub TwitchUserHub
	) : IRequestHandler<ProcessChatMessageCommand>
{

	public async Task Handle
		(ProcessChatMessageCommand command, CancellationToken cancel)
	{
		var chatVm = command.e.ConvertToVm();
		await Signalr.SendChatMessage(chatVm);

		await(Settings.Value.DebugMode is true
			? DebugProcess(command.e)
			: Process(command.e));
	}


	// == 🐌 DEBUG == //

	public async Task DebugProcess(ChatMessageEvent e)
	{
		//if (e.TwitchUserDto.Name is not "ElysiaGriffin") { return; }

		var user = await TwitchUserHub.Start(e.TwitchUserDto);

		if (e.HasCommand())
		{
			var chatCommand = new ChatMessageCommand()
				.Set(user, e);

			await ChatCommandProcessor.Start(chatCommand);
		}

		//if (e.DandleWordSuggestion() && Cache.DandleAcceptingSuggestions())
		//{
		//	// do dandle things
		//}
	}

	// == ⚫ LIVE == //

	public async Task Process(ChatMessageEvent e)
	{
		var user = await TwitchUserHub.Start(e.TwitchUserDto);

		if (e.HasCommand())
		{
			var command = new ChatMessageCommand().Set(user, e);

			await ChatCommandProcessor.Start(command);
		}
	}
}
