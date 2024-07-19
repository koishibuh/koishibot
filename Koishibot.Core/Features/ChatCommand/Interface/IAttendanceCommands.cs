using Koishibot.Core.Features.ChatCommand.Models;

namespace Koishibot.Core.Features.ChatCommand.Interface;

public interface IAttendanceCommands
{
	Task<bool> Process(ChatMessageCommand c);
}