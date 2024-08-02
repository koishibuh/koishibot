using Koishibot.Core.Features.ChatCommands.Interface;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage;
namespace Koishibot.Core.Features.ChatMessages.Events;



/// <summary>
/// Processes chat messages: <br/>
/// Publish to UI, Log User, Save Attendance if Enabled, Check Command
/// </summary>
public record ChatMessageReceived(
	IOptions<Settings> Settings,
	IAppCache Cache,
	ISignalrService Signalr,
	IChatCommandProcessor ChatCommandProcessor,
	ITwitchUserHub TwitchUserHub
	) : IRequestHandler<ChatMessageReceivedCommand>
{
	public async Task Handle
		(ChatMessageReceivedCommand command, CancellationToken cancel)
	{
		var chatVm = command.args.ConvertToVm();
		await Signalr.SendChatMessage(chatVm);

		await (Settings.Value.DebugMode is true
				? DebugProcess(command)
				: Process(command));
	}


	// == 🐌 DEBUG == //

	public async Task DebugProcess(ChatMessageReceivedCommand e)
	{
		//if (e.TwitchUserDto.Name is not "ElysiaGriffin") { return; }
		var userDto = new TwitchUserDto(e.args.ChatterId, e.args.ChatterLogin, e.args.ChatterName);

		var user = await TwitchUserHub.Start(userDto);
		var chat = e.args.ConvertToDto(user);

		if (chat.HasCommand())
		{
			await ChatCommandProcessor.Start(chat);
		}

		//if (e.DandleWordSuggestion() && Cache.DandleAcceptingSuggestions())
		//{
		//	// do dandle things
		//}
	}

	// == ⚫ LIVE == //

	public async Task Process(ChatMessageReceivedCommand e)
	{
		var userDto = new TwitchUserDto(e.args.ChatterId, e.args.ChatterLogin, e.args.ChatterName);
		var user = await TwitchUserHub.Start(userDto);
		var chat = e.args.ConvertToDto(user);

		if (chat.HasCommand())
		{
			await ChatCommandProcessor.Start(chat);
		}
	}
}

// == ⚫ COMMAND == //

public record ChatMessageReceivedCommand(
	ChatMessageReceivedEvent args
	) : IRequest
{

};

