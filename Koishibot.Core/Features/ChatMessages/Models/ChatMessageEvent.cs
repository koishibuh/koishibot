using Koishibot.Core.Features.TwitchUsers.Models;
using TwitchLib.Client.Events;
namespace Koishibot.Core.Features.ChatMessages.Models;

// == ⚫ HANDLER == //


public class ChatMessageEvent
{
	public TwitchUserDto TwitchUserDto { get; set; } = null!;
	public List<KeyValuePair<string, string>> Badges { get; set; } = [];
	public bool IsVip { get; set; }
	public bool IsSubscriber { get; set; }
	public bool IsModerator { get; set; }
	public string Color { get; set; } = "#000000";
	public string? Command { get; set; }
	public string Message { get; set; } = string.Empty;

	// == ⚫ == //

	public ChatMessageEvent Set(OnMessageReceivedArgs e, string? command, string message)
	{
		TwitchUserDto = new TwitchUserDto
			(e.ChatMessage.UserId, e.ChatMessage.Username, e.ChatMessage.DisplayName);
		Badges = e.ChatMessage.Badges;
		IsVip = e.ChatMessage.UserDetail.IsVip;
		IsSubscriber = e.ChatMessage.UserDetail.IsSubscriber;
		IsModerator = e.ChatMessage.UserDetail.IsModerator;
		Color = e.ChatMessage.HexColor;
		Command = command;
		Message = message;
		return this;
	}

	public ChatMessageEvent Set(OnMessageSentArgs e, string? command, string message)
	{
		TwitchUserDto = new TwitchUserDto("98683749", "elysiagriffin", e.SentMessage.DisplayName);
		Badges = e.SentMessage.Badges;
		IsVip = false;
		IsSubscriber = true;
		IsModerator = false;
		Color = e.SentMessage.HexColor;
		Command = command;
		Message = message;
		return this;
	}

	public bool HasCommand()
	{
		return Command is not null;
	}

	public bool DandleWordSuggestion()
	{
		return Message.Length is 5 ? true : false;
	}

	public ChatMessageVm ConvertToVm()
	{
		var originalMessage = Message;

		if (Command is not null)
		{
			originalMessage = Message is ""
				?	originalMessage = $"!{Command}" 
				:	originalMessage = $"!{Command} {Message}";
		}

		return new ChatMessageVm(TwitchUserDto.TwitchId, TwitchUserDto.Name,
						Badges, Color, originalMessage);
	}
}
