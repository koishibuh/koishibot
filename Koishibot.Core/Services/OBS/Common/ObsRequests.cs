namespace Koishibot.Core.Services.OBS.Common;

/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#requests">Obs Documentation</see>
/// </summary>
public class ObsRequests
{
	// SCENES 

	public const string GetCurrentProgramScene = "GetCurrentProgramScene";
	public const string SetCurrentProgramScene = "SetCurrentProgramScene";

	// SOURCES

	public const string SetSceneItemEnabled = "SetSceneItemEnabled";
	public const string GetSceneList = "GetSceneList";
	public const string GetSceneItemList = "GetSceneItemList";

	public const string GetSourceScreenshot = "GetSourceScreenshot";

	// INPUTS

	public const string GetInputList = "GetInputList";
	public const string GetInputKindList = "GetInputKindList";
	public const string OpenInputInteractDialog = "OpenInputInteractDialog";

	// STREAM

	public const string StartStream = "StartStream";
	public const string StopStream = "StopStream";

}

// Program: Current, Preview: TransitioningTo