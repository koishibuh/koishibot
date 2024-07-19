using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Persistence;

namespace Koishibot.Core.Features.Supports.Extensions;
public static class SupportExtensions
{

	// MODEL

	public static StreamEventVm ConvertToVm(this ChannelFollow follow)
	{
		return new StreamEventVm
		{
			EventType = StreamEventType.Follow,
			Timestamp = (DateTimeOffset.UtcNow).ToString("yyyy-MM-dd HH:mm"),
			Message = $"{follow.TwitchUser.Name} has followed"
		};
	}

	public static StreamEventVm ConvertToVm(this TwitchCheer cheer, string username)
	{
		return new StreamEventVm
		{
			EventType = StreamEventType.Cheer,
			Timestamp = (DateTimeOffset.UtcNow).ToString("yyyy-MM-dd HH:mm"),
			Message = $"{username} has cheered {cheer.BitsAmount}"
		};
	}

	// DATABASE

	public static async Task AddFollow(this KoishibotDbContext database, ChannelFollow follow)
	{
		database.Update(follow);
		await database.SaveChangesAsync();
	}

	public static async Task AddCheer(this KoishibotDbContext database, TwitchCheer cheer)
	{
		database.Update(cheer);
		await database.SaveChangesAsync();
	}
}
