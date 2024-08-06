namespace Koishibot.Core.Services.OBS.Authentication;

/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#hello-opcode-0">Obs Documentation</see>
/// </summary>
public class HelloResponse
{
	public string? ObsWebSocketVersion { get; set; }
	public int RpcVersion { get; set; }
	public Authentication? Authentication { get; set; }
}


public class Authentication
{
	public string Challenge { get; set; } = string.Empty;
	public string Salt { get; set; } = string.Empty;
}