using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.AttendanceLog.Interfaces;

public interface IAttendanceProcessor
{
	Task Start(TwitchUser user);
}
