namespace Koishibot.Core.Services.OBS.Scenes;

public class GetSceneItemListRequest
{
	public string SceneName { get; set; }	= string.Empty;
}


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