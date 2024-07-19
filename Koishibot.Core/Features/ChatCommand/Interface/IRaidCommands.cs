using Koishibot.Core.Features.ChatCommand.Models;
namespace Koishibot.Core.Features.ChatCommand.Interface;

public interface IRaidCommands
{
	Task Process(ChatMessageCommand cc);
}