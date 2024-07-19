using Koishibot.Core.Features.ChatCommand.Models;

namespace Koishibot.Core.Features.ChatCommand.Interface;

public interface IDandleCommands
{
	Task<bool> Process(ChatMessageCommand c);
}
