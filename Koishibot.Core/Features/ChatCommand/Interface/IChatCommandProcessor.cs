using Koishibot.Core.Features.ChatCommand.Models;
namespace Koishibot.Core.Features.ChatCommand.Interface;

public interface IChatCommandProcessor
{
	Task Start(ChatMessageCommand c);
}