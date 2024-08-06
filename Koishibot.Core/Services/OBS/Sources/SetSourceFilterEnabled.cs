namespace Koishibot.Core.Services.OBS.Sources;

/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#setsourcefilterenabled">Obs Documentation</see>
/// </summary>
public class SetSourceFilterEnabledRequest
{
	public string SourceName { get; set; } = string.Empty;
	public string SourceUuid { get; set; } = string.Empty;
	public string FilterName { get; set; } = string.Empty;
	public bool FilterEnabled { get; set; }
}

/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#sourcefilterenablestatechanged">Obs Documentation</see>
/// </summary>
public class SourceFilterEnableStateChange
{
	public string? SourceName { get; set; }
	public string? FilterName { get; set; }	
	public bool FilterEnabled { get; set; }
}
