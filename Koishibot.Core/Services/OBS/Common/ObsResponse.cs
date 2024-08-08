namespace Koishibot.Core.Services.OBS.Common;

public class RequestResponse<T>
{
	public string? RequestType { get; set; }
	public string? RequestId { get; set; }
	public T? ResponseData { get; set; }
}