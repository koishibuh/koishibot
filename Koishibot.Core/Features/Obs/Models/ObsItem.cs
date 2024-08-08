namespace Koishibot.Core.Features.Obs.Models;
public class ObsItem
{
	public int Id { get; set; }
	public string Type { get; set; } = string.Empty;
	public string ObsName { get; set; } = string.Empty;
	public string ObsId { get; set; } = string.Empty;
	public string? AppName { get; set; }
}

public class ObsItemType
{
	public const string Scene = "Scene";
	public const string Source = "Source";
	public const string Audio = "Audio";
}

public class ObsAppName
{
	public const string StartScene = "StartScene";
	public const string BRBScene = "BRBScene";
	public const string RaidScene = "RaidScene";
}