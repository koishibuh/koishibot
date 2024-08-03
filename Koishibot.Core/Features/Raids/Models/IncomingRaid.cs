using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.Raids.Models;

public class IncomingRaid
{
	public int Id { get; set; }
	public int TwitchStreamId { get; set; }
	public DateTimeOffset Timestamp { get; set; }
	public int RaidedByUserId { get; set; }
	public int ViewerCount { get; set; }
	public TwitchStream TwitchStream { get; set; } = null!;
	public TwitchUser RaidedByUser { get; set; } = null!;

	//

	public IncomingRaid Set(int streamSessionId, int userId, int viewerCount)
	{
		TwitchStreamId = streamSessionId;
		Timestamp = DateTimeOffset.UtcNow;
		RaidedByUserId = userId;
		ViewerCount = viewerCount;
		return this;
	}
}