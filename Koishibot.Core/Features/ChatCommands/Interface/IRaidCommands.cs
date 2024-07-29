using Koishibot.Core.Features.ChatMessages.Models;
namespace Koishibot.Core.Features.ChatCommands.Interface;

public interface IRaidCommands
{
	Task Process(ChatMessageDto c);
}