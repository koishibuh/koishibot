using Koishibot.Core.Features.ChatMessages.Models;

namespace Koishibot.Core.Features.ChatCommands.Interface;

public interface IDandleCommands
{
	Task<bool> Process(ChatMessageDto c);
}
