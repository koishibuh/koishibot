﻿using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.Twitch.Common;

namespace Koishibot.Core.Features.ChatMessages.Models;

// == ⚫ HANDLER == //


public class ChatMessageDto
{
	public TwitchUser User { get; set; } = null!;
	public List<Badge> Badges { get; set; } = [];
	//public bool IsVip { get; set; }
	//public bool IsSubscriber { get; set; }
	//public bool IsModerator { get; set; }
	public string Color { get; set; } = "#000000";
	public string Message { get; set; } = string.Empty;
	public string? Command { get; set; }

	// == ⚫ == //


	public bool HasCommand()
	{
		return Command is not null;
	}

	public bool DandleWordSuggestion()
	{
		return Message.Length is 5 ? true : false;
	}

	//public ChatMessageVm ConvertToVm()
	//{
	//	var originalMessage = Message;

	//	if (Command is not null)
	//	{
	//		originalMessage = Message is ""
	//			?	originalMessage = $"!{Command}" 
	//			:	originalMessage = $"!{Command} {Message}";
	//	}

	//	return new ChatMessageVm(TwitchUserDto.TwitchId, TwitchUserDto.Name,
	//					new List<KeyValuePair<string, string>>(), Color, originalMessage);
	//}


	/// <summary>
	/// Streamer name is lowercase
	/// </summary>
	/// <returns></returns>
	public string GetSuggestedStreamerFromMessage()
	{
		return Message.Split(' ')[0].TrimStart('@').ToLower();
	}

	public bool SuggestionCommandEmpty()
	{
		return Message.Length == 0;
	}

	public bool CommandIsSuggestion()
	{
		return Command is "suggest";
	}


}
