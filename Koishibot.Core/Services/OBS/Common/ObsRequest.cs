using Koishibot.Core.Services.OBS.Enums;

namespace Koishibot.Core.Services.OBS.Common;
public class ObsRequest<T> where T : class
{
	public OpCodeType? Op { get; set; }

	[JsonPropertyName("d")]
	public RequestWrapper<T>? RequestData { get; set; }
	//public string? RequestId { get; set; }
}

public class ObsRequestSimple
{
	public OpCodeType? Op { get; set; }

	[JsonPropertyName("d")]
	public RequestData RequestData { get; set; } = null!;
}

public class RequestData
{
	public string? RequestType { get; set; }
	public Guid? RequestId { get; set; }
}