namespace Koishibot.Core.Services.OBS.Scenes;

/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#setcurrentprogramscene">Obs Documentation</see>
/// </summary>
public class SetCurrentProgramSceneRequest
{
	/// <summary>
	/// Scene name to set as the current program scene
	/// </summary>
	public string SceneName { get; set; } = string.Empty;

	/// <summary>
	/// Scene UUID to set as the current program scene
	/// </summary>
	public string SceneUuid { get; set; } = string.Empty;
}


/// https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#currentprogramscenechanged