using Koishibot.Core.Features.Raids.Enums;

namespace Koishibot.Core.Features.RaidSuggestions;

public static class OutgoingRaidChatReply
{
	public static async Task PostRaidSuggestionsOpen(this IChatMessageService BotIrc)
	{
		await BotIrc.BotSend("Raid suggestions are now open for the next 2 minutes, please use !suggest (user) to nominate a streamer!");
	}

	public static async Task RaidStatus(this IChatMessageService irc, Code code)
	{
		var message = code switch
		{
			Code.NotCurrentlyRaiding => $"Stream is not over yet silly!",

			Code.SuggestionsOpen
					=> $"Raid suggestions are now open for the next 2 minutes, please use !suggest (user) to nominate a streamer!",
			Code.SuggestionsClosed
					=> $"Raid suggestions are now closed. Voting on a raid target soon!",

			Code.RaidCancelled => $"ABORT THE RAID",
			Code.RaidSuggestionCancelled => $"ABORT THE RAID SUGGESTION AHH",
			_ => $"Something bork on PostRaidStatus lol"
		};

		await irc.BotSend(message);
	}

	public static async Task PostSuggestionResult(this IChatMessageService irc, Code code, (string user, string streamer) e)
	{
		var message = code switch
		{
			Code.SuggestionSuccessful	=> $"{e.user}, thanks for the suggestion! Added {e.streamer} to the list",
			Code.NoSuggestionMade			=> $"{e.user}, Please add a streamer name after !suggest command",
			Code.CantSuggestMe				=> $"{e.user}, you can't add {e.streamer} because it is FORBIDDEN",
			Code.DupeSuggestion				=> $"{e.user}, {e.streamer} has already been suggested - so popular!",
			Code.NotValidUser					=> $"{e.user}, {e.streamer} is not a valid user, check spelling?",
			Code.StreamerOffline			=> $"{e.user}, {e.streamer} is sadly offline",
			Code.ChatIsRestricted			=> $"{e.user}, {e.streamer} has their chat set to follower or sub only mode",
			Code.MaxViewerCount				=> $"{e.user}, please suggest a smoll streamer",
			Code.RaidSuggestionsClosed => $"{e.user}, Raid suggestions are now closed - sorry!",
			_ => $"Something bork on PostSuggestionResult lol"
		};

		await irc.BotSend(message);
	}

	public static async Task PostRaidCall(this IChatMessageService irc)
	{
		var message = $"SabaPing E-LEE-SHA GRIFFIN'S KOI KIN RAID‽ SabaPing";
		await irc.BotSend(message);
	}

	public static async Task RaidTarget(this IChatMessageService irc, Code code, string user, string? streamer = "")
	{
		var message = code switch
		{
			Code.WinningRaidTarget
					=> $"Congrats {user}, {streamer} has been chosen by the koimmunity! Here's some brownie points 🍪",

			Code.RaidLink => $"Get ready to raid! SOON, we'll be heading over to https://www.twitch.tv/{streamer}",
			Code.RaidLeftBehind => $"Sorry if you got left behind! See you over at https://www.twitch.tv/{streamer}",

			Code.RaidAgain
					=> $"Due to shenanigans, we will be raiding {streamer} instead as suggested by {user} | https://www.twitch.tv/{streamer}",

			_ => $"Something bork on PostRaidTarget lol"
		};

		await irc.BotSend(message);
	}
}
