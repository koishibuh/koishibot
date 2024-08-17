using Koishibot.Core.Features.TwitchUsers.Models;
namespace Koishibot.Core.Features.ChannelPoints.Models;

public class ItemTag
{
	public int Id { get; set; }
	public int WordPressId { get; set; }
	public int? UserId { get; set; }

	// NAVIGATION

	public TwitchUser? TwitchUser { get; set; }
}