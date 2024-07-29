namespace Koishibot.Core.Services.Twitch.Irc.Models;

public record TwitchMessage
{
	public int UserId { get; set; }
	public string? UserLogin { get; set; }
	public string? Username { get; set; }
	public string? Color { get; set; } = "#000000";
	public bool IsFirstMessage { get; set; }
	public Guid? MessageId { get; set; }
	public bool IsMod { get; set; }
	public bool IsVip { get; set; }
	public bool IsSubscriber { get; set; }
	public string? Message { get; set; }
	public bool IsCommand => Message?.StartsWith("!") ?? false;

	public bool IsBot()
	{
		if (Username is null)
		{
			return false;
		}
		var parsed = string.Join("", Username.TakeWhile(x => x != '!')).Replace(":", "");
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