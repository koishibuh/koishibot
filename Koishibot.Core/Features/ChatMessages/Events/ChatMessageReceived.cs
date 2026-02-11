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
public record ChatMessageReceivedHandler(
ISignalrService Signalr,
IChatCommandProcessor ChatCommandProcessor,
ITwitchUserHub TwitchUserHub,
ITimeoutUserService TimeoutUserService,
IChatReplyService BotIrc
) : IChatMessageReceivedHandler
{
	public async Task Handle
	(ChatMessageReceivedEvent e)
	{
		var chatVm = e.CreateVm();
		await Signalr.SendChatMessage(chatVm);

		var userDto = e.CreateUserDto();
		var user = await TwitchUserHub.Start(userDto);
		var chat = e.CreateChatMessageDto(user);

		if (chat.HasSpamLink() && chat.UserNotAllowed())
		{
			await TimeoutUserService.TwoSeconds(chat.User.TwitchId);
			await BotIrc.CreateResponse(Reponse.LinkNotPermitted, new { user = chat.User.Name });
		}

		if (chat.HasCommand())
		{
			await ChatCommandProcessor.Start(chat);
		}
	}

/*════════════════════【 🐌 DEBUG 】════════════════════*/
	private async Task DebugProcess(ChatMessageReceivedEvent command)
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
	private async Task Process(ChatMessageReceivedEvent e)
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

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static partial class ChatMessageReceivedEventExtensions
{
	public static ChatMessageVm CreateVm(this ChatMessageReceivedEvent e) =>
		new ChatMessageVm(e.ChatterId, e.ChatterName, new List<KeyValuePair<string, string>>(), 
			e.Color, e.Message.Text ?? string.Empty);
	
	public static TwitchUserDto CreateUserDto(this ChatMessageReceivedEvent e) =>
		new(e.ChatterId, e.ChatterLogin, e.ChatterName);
	
	public static ChatMessageDto CreateChatMessageDto(this ChatMessageReceivedEvent e, TwitchUser user)
	{
		string? command = null;
		var message = "";

		var trimmed = e.Message.Text;

		if (trimmed.StartsWith("!"))
		{
			var match = MessageContainsExclamation().Match(e.Message.Text);
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
			Badges = e.Badges,
			Color = e.Color,
			Message = message,
			Command = command
		};
	}

	[GeneratedRegex(@"^\s*!+(?<command>\w+)(?:\s+(?<message>.+))?$")]
	public static partial Regex MessageContainsExclamation();
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IChatMessageReceivedHandler
{
	Task Handle(ChatMessageReceivedEvent e);
}