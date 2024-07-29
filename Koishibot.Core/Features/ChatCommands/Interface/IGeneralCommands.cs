using Koishibot.Core.Features.ChatMessages.Models;
namespace Koishibot.Core.Features.ChatCommands.Interface;
public interface IGeneralCommands
{
	Task<bool> Process(ChatMessageDto cc);
}