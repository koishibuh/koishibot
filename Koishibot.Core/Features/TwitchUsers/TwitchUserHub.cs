//using Koishibot.Core.Features.AttendanceLog.Interfaces;
//using Koishibot.Core.Features.StreamInformation.Extensions;
//using Koishibot.Core.Features.TwitchUsers.Interfaces;
//using Koishibot.Core.Features.TwitchUsers.Models;
//namespace Koishibot.Core.Features.TwitchUsers;

//public record TwitchUserHub(
//	ITwitchUserRegistration TwitchUserRegistration,
//	IAttendanceProcessor AttendanceProcessor,
//	IAppCache Cache, IOptions<Settings> Settings
//	) : ITwitchUserHub
//{
//	public async Task<TwitchUser> Start(TwitchUserDto userDto)
//	{
//		var (user, notCached) = await TwitchUserRegistration.Start(userDto);
//		if (notCached && Cache.StreamOnline())
//		{
//			if (Settings.Value.DebugMode is true) { return user; }
//			await AttendanceProcessor.Start(user);
//		}

//		return user;
//	}
//}