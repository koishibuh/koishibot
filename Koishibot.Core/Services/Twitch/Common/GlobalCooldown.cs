namespace Koishibot.Core.Services.Twitch.Common;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#global-cooldown">Twitch Documentation</see>
/// </summary>
public class GlobalCooldown
{
	///<summary>
	///Is the setting enabled.
	///</summary>
	[JsonPropertyName("is_enabled")]
	public bool IsEnabled { get; set; }

	///<summary>
	///The cooldown in seconds.
	///</summary>
	[JsonPropertyName("seconds")]
	//[JsonConverter(typeof(TimeSpanSecondsConverter))]
	public int CooldownInSeconds { get; set; }
}

