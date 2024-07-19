using Koishibot.Core.Features.TwitchUsers.Models;
namespace Koishibot.Core.Features.Supports.Models;

public class ChannelFollow
{
	public int Id { get; set; }
	public DateTimeOffset Timestamp { get; set; }
	public int UserId { get; set; }

	// == ⚫ NAVIGATION == //

	public TwitchUser TwitchUser { get; set; } = null!;

	// == ⚫ METHODS == //

	public ChannelFollow Initialize(TwitchUser user)
	{
		Timestamp = DateTimeOffset.UtcNow;
		TwitchUser = user;
		return this;
	}
}