namespace Koishibot.Core.Features.ChannelPoints.Enums;
public class Command
{
	public const string DragonEggQuestEnabled = "dragonquest-enabled";
	public const string DragonEggQuestDisabled = "dragonquest-disabled";
	public const string DragonEggQuestSuccessful = "dragonquest-successful";
	public const string DragonEggQuestFailed = "dragonquest-failed";

	/// <summary>
	/// Template: User, Choice1, Choice2, Choice3
	/// </summary>
	public const string DragonEggQuestPickEgg = "dragonquest-pickegg";
}