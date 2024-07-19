using Koishibot.Core.Features.ChatCommand.Models;
namespace Koishibot.Core.Features.ChatCommand.Interface;
public interface IGeneralCommands
{
	Task<bool> Process(ChatMessageCommand cc);
}