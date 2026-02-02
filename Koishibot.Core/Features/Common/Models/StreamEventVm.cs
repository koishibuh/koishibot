using Koishibot.Core.Features.Common.Enums;

namespace Koishibot.Core.Features.Common.Models;
public class StreamEventVm
{
	public StreamEventType EventType { get; set; }
	public string Timestamp { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;

	public StreamEventVm CreateFollowEvent(string username)
	{
		EventType = StreamEventType.Follow;
		Timestamp = CurrentTime();
		Message = $"{username} has followed";
		return this;
	}

	public StreamEventVm CreateCheerEvent(string username, int amount)
	{
		EventType = StreamEventType.Cheer;
		Timestamp = CurrentTime();
		Message = $"{username} has cheered {amount}";
		return this;
	}
	
	public string CurrentTime() => 
		(DateTimeOffset.UtcNow).ToString("yyyy-MM-dd HH:mm");
};
