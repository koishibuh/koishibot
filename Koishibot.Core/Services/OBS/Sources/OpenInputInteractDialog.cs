namespace Koishibot.Core.Services.OBS.Sources;


/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#openinputinteractdialog">Obs Documentation</see>
/// </summary>
public class OpenInputInteractDialog
{
	/// <summary>
	/// Name of the input to open the dialog of
	/// </summary>
	public string? InputName { get; set; }

	/// <summary>
	/// UUID of the input to open the dialog of 	
	/// </summary>
	public string? InputUuid { get; set; }
}