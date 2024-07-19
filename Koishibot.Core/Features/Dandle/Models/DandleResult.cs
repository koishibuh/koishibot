using Koishibot.Core.Features.StreamInformation.Models;
namespace Koishibot.Core.Features.Dandle.Models;

public class DandleResult
{
	public int Id { get; set; }
	public int TwitchStreamId { get; set; }
	public DateTimeOffset Timestamp { get; set; }
	public int DandleWordId { get; set; }
	public int GuessCount { get; set; }

	public DandleWord DandleWord { get; set; } = null!;
	public TwitchStream TwitchStream { get; set; } = null!;
}