using Koishibot.Core.Services.Twitch.Irc.Interfaces;

namespace Koishibot.Core.Features.Dandle;
public static class DandleChatReply
{
	public static async Task PostDandleGameStarted(this ITwitchIrcService botIrc)
	{
		await botIrc.BotSend("A new game of Dandle has started, use !guess (5 letter word) to suggest a word");
	}

	public static async Task PostDandleGameEnded(this ITwitchIrcService botIrc)
	{
		await botIrc.BotSend("Dandle game ended");
	}

	public static async Task PostDandleNowVoting
		(this ITwitchIrcService botIrc, string words, int time)
	{
		await botIrc.BotSend($"TwitchVotes !v within {time} seconds | {words}");
	}
}
