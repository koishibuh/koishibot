using Koishibot.Core.Features.ChatMessages.Models;
namespace Koishibot.Core.Features.ChatCommands.Interface;

public interface IChatCommandProcessor
{
	Task Start(ChatMessageDto c);
}