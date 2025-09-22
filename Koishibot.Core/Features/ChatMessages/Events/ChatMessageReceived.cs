using System.Text.RegularExpressions;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatCommands.Interface;
using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Features.Moderation;
using Koishibot.Core.Features.Moderation.Enums;
using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage;

namespace Koishibot.Core.Features.ChatMessages.Events;

/// <summary>
/// Processes chat messages: <br/>
/// Publish to UI, Log User, Save Attendance if Enabled, Check Command
/// </summary>
public record ChatMessageReceived(
ISignalrService Signalr,
IChatCommandProcessor ChatCommandProcessor,
ITwitchUserHub TwitchUserHub,
ITimeoutUserService TimeoutUserService,
IChatReplyService BotIrc
) : IRequestHandler<ChatMessageReceivedCommand>
{
	public async Task Handle
	(ChatMessageReceivedCommand command, CancellationToken cancel)
	{
		var chatVm = command.CreateVm();
		await Signalr.SendChatMessage(chatVm);

		var userDto = command.CreateUserDto();
		var user = await TwitchUserHub.Start(userDto);
		var chat = command.CreateChatMessageDto(user);

		if (chat.HasSpamLink() && chat.UserNotAllowed())
		{
			await TimeoutUserService.TwoSeconds(chat.User.TwitchId);
			await BotIrc.CreateResponse(Command.LinkNotPermitted, new { user = chat.User.Name });
		}

		if (chat.HasCommand())
		{
			await ChatCommandProcessor.Start(chat);
		}
	}

/*════════════════════【 🐌 DEBUG 】════════════════════*/
	private async Task DebugProcess(ChatMessageReceivedCommand command)
	{
		//if (e.TwitchUserDto.Name is not "ElysiaGriffin") { return; }
		var userDto = command.CreateUserDto();

		var user = await TwitchUserHub.Start(userDto);
		var chat = command.CreateChatMessageDto(user);

		if (chat.HasCommand())
		{
			await ChatCommandProcessor.Start(chat);
		}

		//if (e.DandleWordSuggestion() && Cache.DandleAcceptingSuggestions())
		//{
		//	// do dandle things
		//}
	}

/*════════════════════【 🟣 LIVE 】════════════════════*/
	private async Task Process(ChatMessageReceivedCommand e)
	{
		var userDto = e.CreateUserDto();
		var user = await TwitchUserHub.Start(userDto);
		var chat = e.CreateChatMessageDto(user);

		if (chat.HasCommand())
		{
			await ChatCommandProcessor.Start(chat);
		}
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public partial record ChatMessageReceivedCommand(
ChatMessageReceivedEvent E
) : IRequest
{
	public ChatMessageVm CreateVm() =>
	new ChatMessageVm(E.ChatterId, E.ChatterName, new List<KeyValuePair<string, string>>(),
	E.Color, E.Message.Text ?? string.Empty);

	public TwitchUserDto CreateUserDto() =>
	new(E.ChatterId, E.ChatterLogin, E.ChatterName);

	public ChatMessageDto CreateChatMessageDto(TwitchUser user)
	{
		string? command = null;
		var message = "";

		var trimmed = E.Message.Text;

		if (trimmed.StartsWith("!"))
		{
			var match = MessageContainsExclamation().Match(E.Message.Text);
			if (match.Success)
			{
				command = match.Groups["command"].Value.ToLower();
				message = match.Groups["message"].Value;
			}
		}
		else
		{
			message = trimmed;
		}

		return new ChatMessageDto
		{
		User = user,
		Badges = E.Badges,
		Color = E.Color,
		Message = message,
		Command = command
		};
	}

	[GeneratedRegex(@"^\s*!+(?<command>\w+)(?:\s+(?<message>.+))?$")]
	public static partial Regex MessageContainsExclamation();
}