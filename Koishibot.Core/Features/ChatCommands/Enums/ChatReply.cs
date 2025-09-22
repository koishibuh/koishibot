namespace Koishibot.Core.Features.ChatCommands.Enums;

public class Command
{
	// AD

	public const string AdBreak = "adBreak";
	public const string AdNowPlaying = "adNowPlaying";
	public const string AdCompleted = "adCompleted";

	// TODOIST
	public const string Later = "later";
	public const string Bug = "bug";
	public const string Idea = "idea";

	// STREAM
	public const string StreamOnline = "stream-online";
	public const string ChannelUpdated = "channelUpdated";
}

public class Response
{
	public const string Later = "todoist-later";
	public const string Bug = "todoist-bug";
	public const string Idea = "todoist-idea";
}