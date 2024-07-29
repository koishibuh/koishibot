using Koishibot.Core.Features.ChatMessages.Models;

namespace Koishibot.Core.Features.Dandle.Interfaces;
public interface IDandleSuggestionProcessor
{
	Task Start(ChatMessageDto c);
}