using Koishibot.Core.Features.AttendanceLog;
using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.StreamInformation.Extensions;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
namespace Koishibot.Core.Features.TwitchUsers;

/*═══════════════════【 SERVICE 】═══════════════════*/
public record TwitchUserHub(
	ITwitchUserRegistration TwitchUserRegistration,
	IAttendanceProcessor AttendanceProcessor,
	IAppCache Cache, IOptions<Settings> Settings
	) : ITwitchUserHub
{
	public async Task<TwitchUser> Start(TwitchUserDto userDto, bool isFollow = false)
	{
		var (user, notCached) = await TwitchUserRegistration.Start(userDto);
		if (notCached && Cache.StreamOnline())
		{
			if (Settings.Value.DebugMode is true) { return user; }
			if (Cache.AttendanceDisabled()) return user;
			
			await AttendanceProcessor.Start(user, isFollow);
		}

		return user;
	}
}

/*═══════════════════【 INTERFACE 】═══════════════════*/
public interface ITwitchUserHub
{
	Task<TwitchUser> Start(TwitchUserDto userDto, bool isFollow = false);
}