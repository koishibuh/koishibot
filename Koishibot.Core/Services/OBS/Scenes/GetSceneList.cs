namespace Koishibot.Core.Services.OBS.Scenes;

// == ⚫ RESPONSE == //

/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#getscenelist">Obs Documentation</see>
/// </summary>
public class GetSceneListResponse
{
	/// <summary>
	/// Current program scene name.Can be null if internal state desync
	/// </summary>
	public string? CurrentProgramSceneName { get; set; }

	/// <summary>
	/// Current program scene UUID.Can be null if internal state desync
	/// </summary>
	public string? CurrentProgramSceneUuid { get; set; }

	/// <summary>
	///  Current preview scene name. null if not in studio mode
	/// </summary>
	public string? CurrentPreviewSceneName { get; set; }

	/// <summary>
	/// Current preview scene UUID. null if not in studio mode
	/// </summary>
	public string? CurrentPreviewSceneUuid { get; set; }

	/// <summary>
	/// 	Array of scenes
	/// </summary>
	public List<Scene> Scenes { get; set; }
}


public class Scene
{
	/// <summary>
	/// Gets the scene index.
	/// </summary>
	public int SceneIndex { get; set; }

	/// <summary>
	/// Gets the scene name.
	/// </summary>
	public string? SceneName { get; set; }

	/// <summary>
	/// UUID of the scene.
	/// </summary>
	public string? SceneUuid { get; set; }
}
