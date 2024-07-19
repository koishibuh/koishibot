using Koishibot.Core.Features.ChatCommand.Models;

namespace Koishibot.Core.Features.Dandle.Interfaces;
public interface IDandleSuggestionProcessor
{
	Task Start(ChatMessageCommand c);
}