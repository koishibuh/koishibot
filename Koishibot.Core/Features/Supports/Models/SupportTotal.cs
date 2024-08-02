using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.Supports.Models;

public class SupportTotal
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public int MonthsSubscribed { get; set; }
	public int SubsGifted { get; set; }
	public int BitsCheered { get; set; }
	public int Tipped { get; set; }

	// == ⚫ NAVIGATION == //

	public TwitchUser TwitchUser { get; set; } = null!;
}