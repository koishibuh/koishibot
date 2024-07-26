namespace Koishibot.Core.Services.Twitch.Irc;

public record TwitchMessage
{
	public string? DisplayName { get; set; }
	public bool IsFirstMessage { get; set; }
	public Guid? MessageId { get; set; }
	public bool IsMod { get; set; }
	public bool IsSubscriber { get; set; }
	public int UserId { get; set; }
	public string? User { get; set; }
	public string? Message { get; set; }
	public bool IsCommand => Message?.StartsWith("!") ?? false;

	public bool IsBot()
	{
		if (DisplayName is null)
		{
			return false;
		}
		var parsed = string.Join("", DisplayName.TakeWhile(x => x != '!')).Replace(":", "");
		return parsed == "koishibuh" || parsed == "honestdanbot" || parsed == "streamelements";
	}

	public bool IsValidCommand(List<string> commands)
	{
		if (IsCommand && !commands.Contains(Message ?? ""))
		{
			return false;
		}
		return commands.Contains(Message ?? "");
	}
}