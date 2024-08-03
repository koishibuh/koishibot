using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.Kofi.Enums;
namespace Koishibot.Core.Features.Supports.Models;

public class Kofi : IEntity
{
	public int Id { get; set; }
	public string KofiTransactionId { get; set; } = string.Empty;
	public DateTimeOffset Timestamp { get; set; }
	public string TransactionUrl { get; set; } = string.Empty;	
	public KofiType KofiType { get; set; }
	public int? UserId { get; set; }
	public string Username { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;
	public string Currency { get; set; } = string.Empty;
	public string Amount { get; set; } = string.Empty;	

	// NAVIGATION

	public TwitchUser? TwitchUser { get; set; }
}