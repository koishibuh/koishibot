using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.Supports.Models;

public class Subscription : IEntity
{
	public int Id { get; set; }
	public DateTimeOffset Timestamp { get; set; }	
	public int UserId { get; set; }
	public string Tier { get; set; } = string.Empty;
	public string? Message { get; set; }
	public bool Gifted { get; set; }

	// == ⚫ NAVIGATION == //

	public TwitchUser TwitchUser { get; set; } = null!;
}

// Sub, Resub, Gifted Sub, Prime sub