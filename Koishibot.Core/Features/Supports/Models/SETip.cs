using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.Supports.Models;

public class SETip : IEntity
{
	public int Id { get; set; }
	public string StreamElementsId { get; set; } = string.Empty;
	public DateTimeOffset Timestamp { get; set; }
	public int? UserId { get; set; }
	public string Username { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;
	public string Amount { get; set; } = string.Empty;

	// NAVIGATION

	public TwitchUser? TwitchUser { get; set; }
}