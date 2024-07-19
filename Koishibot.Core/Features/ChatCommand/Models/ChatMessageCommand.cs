using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.ChatCommand.Models;

/// <summary>
/// Command is lowercase and without !
/// </summary>
public class ChatMessageCommand
{
	public TwitchUser User = null!;
	public bool IsVip;
	public bool IsSubscriber;
	public bool IsModerator;
	public string Command = string.Empty;
	public string Message = string.Empty;

	// == ⚫ == //

	public bool CommandIsSuggestion()
	{
		return Command is "suggest";
	}

	public bool SuggestionCommandEmpty()
	{
		return Message.Length == 0;
	}

	/// <summary>
	/// Streamer name is lowercase
	/// </summary>
	/// <returns></returns>
	public string GetSuggestedStreamerFromMessage()
	{
		return Message.Split(' ')[0].TrimStart('@').ToLower();
	}

	public ChatMessageCommand Set(TwitchUser user, ChatMessageEvent e)
	{
		User = user;
		IsVip = e.IsVip;
		IsSubscriber = e.IsSubscriber;
		IsModerator = e.IsModerator;
		Command = e.Command!;
		Message = e.Message;

		return this;
	}
};