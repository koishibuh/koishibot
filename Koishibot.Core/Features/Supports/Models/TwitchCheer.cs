using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;

namespace Koishibot.Core.Features.Supports.Models;
public class TwitchCheer
{
	public int Id { get; set; }
	public DateTimeOffset Timestamp { get; set; }	
	public int UserId { get; set; }	
	public int BitsAmount { get; set; }	
	public string Message { get; set; } = string.Empty;

	// == ⚫ NAVIGATION == //

	public TwitchUser TwitchUser { get; set; } = null!;

	// == ⚫ METHODS == //

	public TwitchCheer Initialize(int userId, int bits, string message)
	{
		Timestamp = DateTimeOffset.UtcNow;
		UserId = userId;
		BitsAmount = bits;
		Message = message;

		return this;
	}

	public async Task UpdateRepo(KoishibotDbContext context)
	{
		context.Add(this);
		await context.SaveChangesAsync();
	}
}
