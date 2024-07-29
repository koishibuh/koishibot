﻿using Koishibot.Core.Services.Twitch.Irc.Models;
using System.Text.RegularExpressions;

namespace Koishibot.Core.Services.Twitch.Irc.Extensions;

public static partial class TwitchIrcExtensions
{
	public static bool IsPingMessage(this string ircMessage)
	{
		return ircMessage.Contains("PING :tmi.twitch.tv");
	}

	public static TwitchMessage ToTwitchMessage(this string ircMessage)
	{
		var userLogin = UsernameRegex().Match(ircMessage);
		var message = MessageRegex().Match(ircMessage)?.Groups["Message"]?.Value?.Trim().ToLower();
		var tagMatches = TagRegex().Matches(ircMessage);

		var dict = new Dictionary<string, string>();
		foreach (Match match in tagMatches)
		{
			var item = match.Value;
			var splitItem = item.Split("=");
			dict.TryAdd(splitItem[0], splitItem[1]);
		}

		// TODO: badge list

		dict.TryGetValue(TwitchIrcConstants.UserId, out var userId);
		dict.TryGetValue(TwitchIrcConstants.IsMod, out var isMod);
		dict.TryGetValue(TwitchIrcConstants.FirstMessage, out var isFirstMessage);
		dict.TryGetValue(TwitchIrcConstants.Id, out var messageId);
		dict.TryGetValue(TwitchIrcConstants.Username, out var displayName);
		dict.TryGetValue(TwitchIrcConstants.IsSubscriber, out var isSubscriber);
		dict.TryGetValue(TwitchIrcConstants.Color, out var color);
		dict.TryGetValue(TwitchIrcConstants.IsVip, out var isVip);

		int.TryParse(userId, out var parsedUserId);
		bool.TryParse(isSubscriber, out var parsedIsSubscriber);
		bool.TryParse(isMod, out var parsedIsMod);
		bool.TryParse(isFirstMessage, out var parsedIsFirstMessage);
		bool.TryParse(isVip, out var parsedIsVip);

		var twitchIrcMessage = new TwitchMessage
		{
			UserId = parsedUserId,
			UserLogin = userLogin.Groups[0].Value,
			Username = displayName ?? string.Empty,
			Color = color ?? string.Empty,
			IsFirstMessage = parsedIsFirstMessage,
			MessageId = string.IsNullOrEmpty(messageId) ? null : Guid.Parse(messageId),
			IsMod = parsedIsMod,
			IsVip = parsedIsVip,
			IsSubscriber = parsedIsSubscriber,
			Message = message ?? string.Empty,
		};

		return twitchIrcMessage;
	}

	[GeneratedRegex(@"([\w]+!\w+@\w+.tmi.twitch.tv)")]
	private static partial Regex UsernameRegex();
	[GeneratedRegex(@"(?<EntireMessage>(PRIVMSG)(?<User>\s.*:{1})(?<Message>.+))")]
	private static partial Regex MessageRegex();
	[GeneratedRegex(@"(?<Tags>((\w(-*))+=(\w|#|:|-)+))")]
	private static partial Regex TagRegex();
}