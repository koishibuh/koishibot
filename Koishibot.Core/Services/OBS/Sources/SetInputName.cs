namespace Koishibot.Core.Services.OBS.Sources;

/*═════════════【 REQUEST PARAMETERS 】═════════════*/
/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#setinputname">Obs Documentation</see>
/// </summary>
public class SetInputNameRequest
{
	/// <summary>
	/// Current input name
	/// </summary>
	public string? InputName { get; set; }

	/// <summary>
	/// Current input UUID
	/// </summary>
	public string? InputUuid { get; set; }

	/// <summary>
	/// New name for input
	/// </summary>
	public string? NewInputName { get; set; }
}
