using Koishibot.Core.Services.OBS.Enums;
namespace Koishibot.Core.Services.OBS.Common;

public class ObsResponse<T> where T : class
{
	/// <summary>
	/// WebsocketOpCode
	/// </summary>
	[JsonPropertyName("op")]
	public OpCodeType OpCode { get; set; }

	/// <summary>
	/// Data
	/// </summary>
	[JsonPropertyName("d")]
	public T? Response { get; set; }
}