using Koishibot.Core.Features.ChatMessages.Models;

namespace Koishibot.Core.Features.ChatCommands.Interface;

public interface IAttendanceCommands
{
	Task<bool> Process(ChatMessageDto c);
}