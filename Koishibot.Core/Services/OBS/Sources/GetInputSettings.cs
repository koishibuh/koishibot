using Koishibot.Core.Services.OBS.Sources;
namespace Koishibot.Core.Services.OBS.Inputs;


/*═════════════【 REQUEST PARAMETERS 】═════════════*/
/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#getinputsettings">Obs Documentation</see>
/// </summary>
public class GetInputSettingsRequest
{
	/// <summary>
	/// Name of the input to get the settings of
	/// </summary>
	public string? InputName { get; set; }

	/// <summary>
	/// UUID of the input to get the settings of
	/// </summary>
	public string? InputUuid { get; set; }
}

/*══════════════════【 RESPONSE 】══════════════════*/
public class GetInputSettingsResponse
{
	/// <summary>
	/// Array of inputs
	/// </summary>
	public List<InputSettings> InputSettings { get; set; }

	/// <summary>
	/// The kind of the input
	/// </summary>
	public InputTypes InputKind { get; set; }
}

public class InputSettings
{
	public string InputKind { get; set; }
	public string InputName { get; set; }
	public string InputUuid { get; set; }
	public string UnversionedInputKind { get; set; }
}