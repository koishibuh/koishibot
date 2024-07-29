using Koishibot.Core.Services.Twitch.Enums;

namespace Koishibot.Core.Services.Twitch.Common;

public class Fragments
{
	///<summary>
	///The type of message fragment.
	///</summary>
	[JsonPropertyName("type")]
	public FragmentType Type { get; set; }

	///<summary>
	///Message text in fragment.
	///</summary>
	[JsonPropertyName("text")]
	public string Text { get; set; } = string.Empty;

	///<summary>
	///Optional. Metadata pertaining to the cheermote.  (Automod)
	///</summary>
	[JsonPropertyName("cheermote")]
	public Cheermote? Cheermote { get; set; }

	///<summary>
	///Optional. Metadata pertaining to the emote.
	///</summary>
	[JsonPropertyName("emote")]
	public Emote? Emote { get; set; }

	/// <summary>
	/// Optional. Metadata pertaining to the mention. (ChatNotification)
	/// </summary>
	[JsonPropertyName("mention")]
	public Mention? Mention { get; set; }
}