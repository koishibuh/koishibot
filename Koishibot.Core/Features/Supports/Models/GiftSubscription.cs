using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.Supports.Models;

public class GiftSubscription : IEntity
{
	public int Id { get; set; }
	public DateTimeOffset Timestamp { get; set; }
	public int UserId { get; set; }
	public string Tier { get; set; } = string.Empty;
	public int Total { get; set; }

	// == ⚫ NAVIGATION == //

	public TwitchUser TwitchUser { get; set; } = null!;

	// == ⚫ METHODS == //

	public GiftSubscription Initialize(int userId, string tier, int total)
	{
		Timestamp = DateTimeOffset.UtcNow;
		UserId = userId;
		Tier = tier;
		Total = total;
		return this;
	}

}