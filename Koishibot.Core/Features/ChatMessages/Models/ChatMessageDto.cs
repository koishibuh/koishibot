using System.Text.RegularExpressions;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.Twitch.Common;

namespace Koishibot.Core.Features.ChatMessages.Models;

public partial class ChatMessageDto
{
	public TwitchUser User { get; set; } = null!;

	public List<Badge> Badges { get; set; } = [];

	//public bool IsVip { get; set; }
	//public bool IsSubscriber { get; set; }
	//public bool IsModerator { get; set; }
	public string Color { get; set; } = "#000000";
	public string Message { get; set; } = string.Empty;
	public string? Command { get; set; }


	public bool HasCommand() => Command is not null;

	public bool DandleWordSuggestion() => Message.Length is 5;

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
	public string GetSuggestedStreamerFromMessage() =>
	Message.Split(' ')[0].TrimStart('@').ToLower();

	public bool SuggestionCommandEmpty() => Message.Length == 0;

	public bool CommandIsSuggestion() => Command is "suggest";

	public bool UserNotAllowed() => User.Permissions == PermissionLevel.Everyone;

	public bool MessageHasLink() => UrlRegex().IsMatch(Message);

	public bool HasSpamLink()
	{
		var spam = "ers on";
		if (Message.Contains(spam, StringComparison.OrdinalIgnoreCase) is false) return false;

		const string link = @"\b\w*[\./]\w*$";
		var regex = new Regex(link);

		return regex.IsMatch(Message);
	}

	public bool MessageCorrectLength(int length) => Message.Length == length;

	public bool MessageContainsNonLetters()
	{
		var validCharacters = NonLettersRegex();
		return !validCharacters.IsMatch(Message);
	}

	[GeneratedRegex(@"(https?:\/\/(?:[-\w]+\.)+[a-zA-Z]{2,}(?:\/[^\s]*)?)")]
	private static partial Regex UrlRegex();

	[GeneratedRegex(@"^[a-zA-Z]+$")]
	private static partial Regex NonLettersRegex();
}