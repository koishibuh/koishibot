using OBSStudioClient.Classes;
namespace Koishibot.Core.Services.OBS.Common;

public class ObsData<T>
{
	public Guid RequestId { get; set; }
	public string? RequestType { get; set; }
	public T? ResponseData { get; set; }
	public T? Authentication { get; set; }
	public RequestStatus? RequestStatus { get; set; } 
}