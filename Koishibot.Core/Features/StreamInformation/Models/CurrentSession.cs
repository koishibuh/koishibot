namespace Koishibot.Core.Features.StreamInformation.Models;

public class CurrentSession
{
	public int? LiveStreamId { get; set; }
	public int? StreamSessionId { get; set; }
	public int? LastMandatorySessionId { get; set; }
	public string Summary { get; set; } = "";
}