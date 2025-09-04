namespace Koishibot.Core.Services.Twitch.Common;

public class RewardEmote
{
	/// <summary>
	/// The ID that uniquely identifies this emote.
	/// </summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	/// <summary>
	///The human readable emote token.
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; }
}