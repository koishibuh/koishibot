namespace Koishibot.Core.Services.OBS.Common;
public class RequestWrapper<T>
{
	public string? RequestType { get; set; }
	public Guid? RequestId { get; set; }
	public T? RequestData { get; set; }
	public int? RpcVersion { get; set; }
	public string Authentication { get; set; } = string.Empty;
}