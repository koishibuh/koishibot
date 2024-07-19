using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.Dandle.Models;

public record DandleSuggestion(
	TwitchUser User,
	DandleWord DandleWord,
	List<TwitchUser> SuggestionCounter
)
{
	public List<TwitchUser> SuggestionCounter { get; set; } = SuggestionCounter;
}