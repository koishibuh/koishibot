namespace Koishibot.Core.Services.OBS.Sources;

/*═════════════【 REQUEST PARAMETERS 】═════════════*/
/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#toggleinputmute">Obs Documentation</see>
/// </summary>
public class ToggleInputMuteRequest
{
	/// <summary>
	/// Name of the input to toggle the mute state of
	/// </summary>
	public string? InputName { get; set; }
	/// <summary>
	/// UUID of the input to toggle the mute state of
	/// </summary>
	public string? InputUuid { get; set; }
}

/*══════════════════【 RESPONSE 】══════════════════*/
public class ToggleInputMuteResponse
{
	/// <summary>
	/// Whether the input has been muted or unmuted
	/// </summary>
	public bool InputMuted { get; set; }
}