using Koishibot.Core.Services.Twitch.Enums;
namespace Koishibot.Core.Services.Twitch.Common;

public class Emote
{
	///<summary>
	///An ID that uniquely identifies this emote.
	///</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; } = string.Empty;

	///<summary>
	///An ID that identifies the emote set that the emote belongs to.
	///</summary>
	[JsonPropertyName("emote_set_id")]
	public string? EmoteSetId { get; set; }

	///<summary>
	///The ID of the broadcaster who owns the emote.
	///</summary>
	[JsonPropertyName("owner_id")]
	public string? OwnerId { get; set; }

	///<summary>
	///The formats that the emote is available in.<br/>
	///For example, if the emote is available only as a static PNG, the array contains only static.<br/>
	///If the emote is available as a static PNG and an animated GIF, the array contains static and animated.
	///</summary>
	[JsonPropertyName("format")]
	public List<EmoteFormat>? Format { get; set; }

	/// <summary>
	/// The index of where the Emote starts in the text. (Channel Point, Fragments)
	/// </summary>
	[JsonPropertyName("begin")]
	public int Begin { get; set; }

	/// <summary>
	/// The index of where the Emote ends in the text.  (Channel Point, Fragments)
	/// </summary>
	[JsonPropertyName("end")]
	public int End { get; set; }
}