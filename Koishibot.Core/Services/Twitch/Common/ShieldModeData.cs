using Koishibot.Core.Services.Twitch.Converters;

namespace Koishibot.Core.Services.Twitch.Common;

public class ShieldModeData
{
	///<summary>
	///A Boolean value that determines whether Shield Mode is active.<br/>
	///Is true if the broadcaster activated Shield Mode; otherwise, false.
	///</summary>
	[JsonPropertyName("is_active")]
	public bool IsActive { get; set; }

	///<summary>
	///An ID that identifies the moderator that last activated Shield Mode.<br/>
	///Is an empty string if Shield Mode hasn’t been previously activated.
	///</summary>
	[JsonPropertyName("moderator_id")]
	public string ModeratorId { get; set; }

	///<summary>
	///The moderator’s login name.<br/>
	///Is an empty string if Shield Mode hasn’t been previously activated.
	///</summary>
	[JsonPropertyName("moderator_login")]
	public string ModeratorLogin { get; set; }

	///<summary>
	///The moderator’s display name.<br/>
	///Is an empty string if Shield Mode hasn’t been previously activated.
	///</summary>
	[JsonPropertyName("moderator_name")]
	public string ModeratorName { get; set; }

	///<summary>
	///The timestamp of when Shield Mode was last activated.<br/>
	///Is an empty string if Shield Mode hasn’t been previously activated.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("last_activated_at")]
	public DateTimeOffset? LastActivatedAt { get; set; }
}