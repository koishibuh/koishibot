using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.AttendanceLog.Interfaces;

public interface IUpdateAttendanceOptService
{
	Task Start(bool status, TwitchUser user);
}