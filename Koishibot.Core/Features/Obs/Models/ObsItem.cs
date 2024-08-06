namespace Koishibot.Core.Features.Obs.Models;
public class ObsItem
{
	public int Id { get; set; }
	public string Type { get; set; }
	public ObsItemType Name { get; set; }
	public string ObsId { get; set; }
}

public class ObsItemType
{
	public const string Scene = "Scene";
	public const string Source = "Source";
	public const string Audio = "Audio";
}