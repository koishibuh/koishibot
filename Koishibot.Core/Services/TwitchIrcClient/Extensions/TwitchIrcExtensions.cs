namespace Koishibot.Core.Services.TwitchIrcClient.Extensions;
public static class TwitchIrcExtensions
{
	public static bool IsPingMessage(this string ircMessage)
	{
		return ircMessage.Contains("PING :tmi.twitch.tv");
	}
}
