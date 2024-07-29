using Koishibot.Core.Features.ChatMessages.Models;
namespace Koishibot.Core.Features.Dandle.Interfaces;

public interface IDandleWordService
{
	Task CreateWord(ChatMessageDto c);
	Task DeleteWord(ChatMessageDto c);
	Task DefineWord(string word);
}