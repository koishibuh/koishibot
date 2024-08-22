namespace Koishibot.Core.Services.OBS.Scenes;

// Need to specify a scene name or uuid. Would need to fetch list of scenes
// then query for item list

/*════════════════【 REQUEST BODY 】════════════════*/
/// <summary>
/// https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#getsceneitemlist
/// </summary>
public class GetSceneItemListRequest
{
	public string? SceneName { get; set; }
	public string? SceneUuid { get; set; }
}

/*══════════════════【 RESPONSE 】══════════════════*/
public class GetSceneItemListResponse
{
	public List<SceneItem> SceneItems { get; set; } = [];
}

public class SceneItem
{
	public string? SourceName { get; set; }
	public string? SourceType { get; set; }
	public int SceneItemId { get; set; }
	public bool SceneItemEnabled { get; set; }
}