namespace Koishibot.Core.Features.ChannelPoints.Enums;
public class Command
{
	
}

public class Response
{
	public const string DragonQuestEnabled = "dragonquest-enabled";
	public const string DragonQuestDisabled = "dragonquest-disabled";
	public const string DragonQuestSuccessful = "dragonquest-successful";
	public const string DragonQuestFailed = "dragonquest-failed";
	public const string DragonQuestClosed = "dragonquest-nowclosed";

	/// <summary>
	/// Template: User, Choice1, Choice2, Choice3
	/// </summary>
	public const string DragonQuestPickEgg = "dragonquest-pickegg";

	/// <summary>
	///  Template: Url
	/// </summary>
	public const string DragonQuestNewestEgg = "dragonquest-newegg";
}