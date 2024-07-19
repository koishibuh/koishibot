using Koishibot.Core.Features.ChatCommand.Models;
namespace Koishibot.Core.Features.Dandle.Interfaces;

public interface IDandleWordService
{
	Task CreateWord(ChatMessageCommand c);
	Task DeleteWord(ChatMessageCommand c);
	Task DefineWord(string word);
}