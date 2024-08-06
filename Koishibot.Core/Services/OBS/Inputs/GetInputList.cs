namespace Koishibot.Core.Services.OBS.Inputs;

/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#getinputlist">Obs Documentation</see>
/// </summary>
public class GetInputListRequest
{
	/// <summary>
	/// Optional. Restrict the array to only inputs of the specified kind
	/// </summary>
	public string? InputKind { get; set; }
}

public class GetInputListResponse
{
	/// <summary>
	/// Array of inputs
	/// </summary>
	public List<object> Inputs { get; set; }
}