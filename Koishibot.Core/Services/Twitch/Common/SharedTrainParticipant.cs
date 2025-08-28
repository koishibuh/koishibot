namespace Koishibot.Core.Services.Twitch.Common;

public class SharedTrainParticipant
{
	///<summary>
	///The ID of the broadcaster participating in the Shared Hype Train.
	///</summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string BroadcasterId { get; set; }
	
	///<summary>
	///The login of the broadcaster participating in the Shared Hype Train
	///</summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string BroadcasterLogin { get; set; }
	
	///<summary>
	///The username of the broadcaster participating in the Shared Hype Train
	///</summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string BroadcasterUsername { get; set; }
}
