namespace Koishibot.Core.Services.OBS.Sources;

/*═════════════【 REQUEST PARAMETERS 】═════════════*/
/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#setinputsettings">Obs Documentation</see>
/// </summary>
public class SetInputSettingsRequest<T>
{
	/// <summary>
	/// Name of the input to set the settings of
	/// </summary>
	public string? InputName { get; set; }

	/// <summary>
	/// UUID of the input to set the settings of
	/// </summary>
	public string? InputUuid { get; set; }

	/// <summary>
	/// Object of settings to apply
	/// </summary>
	public T InputSettings { get; set; }

	/// <summary>
	/// True == apply the settings on top of existing ones, False == reset the input to its defaults, then apply settings.
	/// </summary>
	// public bool? Overlay { get; set; }
}

public class BrowserSourceSettings
{
	public int? Height { get; set; }
	public int? Width { get; set; }
	public string? Url { get; set; }
}